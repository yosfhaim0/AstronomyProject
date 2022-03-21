using ApiRequests.Nasa;
using DataAccess.DbContexts;
using Microsoft.EntityFrameworkCore;
using Models;
using Models.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Tools;

namespace DataAccess.Repositories
{
    public class NearAsteroidRepository : EFModelRepository<NearAsteroid>, INearAsteroidRepository
    {
        readonly NasaApi _naseApi;

        public AstronomyContext MyContext { get => Context as AstronomyContext; }

        public NearAsteroidRepository(AstronomyContext context, NasaApi nasaApi) : base(context)
        {
            _naseApi = nasaApi;
        }

        public async Task<IEnumerable<NearAsteroid>> GetNearAsteroids(Expression<Func<NearAsteroid, bool>> predicate = null)
        {
            var asteroids = await MyContext
                .NearAsteroids
                .Include(a => a.CloseApproachs)
                .Where(predicate is null ? _ => true : predicate)
                .ToListAsync();
            await FillAsteroidsWithCloseApprochData(asteroids);
            return asteroids;
        }

        public async Task<IEnumerable<NearAsteroid>> ClosestApproachBetweenDates(DateTime startDate, DateTime endDate = default)
        {
            try
            {
                var asteroids = await MyContext.NearAsteroids
                     .Include(a => a.CloseApproachs)
                     .Where(a => a.CloseApproachs.Any(c =>
                      c.CloseApproachDate.Date <= endDate.Date
                       && c.CloseApproachDate.Date >= startDate.Date))
                     .ToListAsync();

                if (!asteroids.Any() || !await IsDbContainCloseApproach(startDate, endDate))
                {
                    await GetNewAsteroidsFromNasa(startDate, endDate, asteroids);
                }

                await FillAsteroidsWithCloseApprochData(asteroids, endDate);

                return asteroids;
            }
            catch (Exception)
            {

                throw;
            }
        }

        private async Task FillAsteroidsWithCloseApprochData(List<NearAsteroid> asteroids, DateTime endDate = default)
        {
            var newAstroidsFromNasa = await GetCloseApprochDataFromNasa(asteroids, endDate);

            await InsertNewCloseApprochsToDb(asteroids, endDate, newAstroidsFromNasa);
        }

        private async Task InsertNewCloseApprochsToDb(List<NearAsteroid> asteroids, DateTime endDate, GetNearAsteroidNasaDto[] newAstroidsFromNasa)
        {
            var tasks = new List<Task>();
            foreach (var astFromNasa in newAstroidsFromNasa)
            {
                var currAstFromDb = await GetById(astFromNasa.Id);
                var currNewAst = asteroids.Find(a => a.Id == astFromNasa.Id);
                if (currAstFromDb == null || currNewAst == null)
                {
                    continue;
                }

                var isNotFill = !await IsAsteroidFill(currAstFromDb.Id);

                var newCA = new List<CloseApproach>();
                if (isNotFill)
                {
                    newCA = astFromNasa.CloseApproachs
                        .Except(currNewAst.CloseApproachs)
                        .ToList();
                }
                else if (endDate != default)
                {   // Get the new observations of close approachs
                    newCA = (from c in astFromNasa.CloseApproachs
                             where c.CloseApproachDate.Date > endDate.Date
                             select c)
                             .ToList();
                }
                if (!newCA.Any())
                {
                    continue;
                }

                currAstFromDb.CloseApproachs.AddRange(newCA);
                currNewAst.CloseApproachs.AddRange(newCA);

                tasks.Add(MyContext.CloseApproachs.AddRangeAsync(newCA));
            }
            await Task.WhenAll(tasks);
        }

        private async Task<GetNearAsteroidNasaDto[]> GetCloseApprochDataFromNasa(List<NearAsteroid> asteroids, DateTime endDate)
        {
            var tasks = new List<Task<GetNearAsteroidNasaDto>>();
            foreach (var a in asteroids)
            {
                bool isDbContainCloseApproach = endDate != default && await IsDbContainCloseApproachById(endDate, a.Id);
                bool isAsteroidFill = await IsAsteroidFill(a.Id);
                if (!isDbContainCloseApproach || !isAsteroidFill)
                {
                    tasks.Add(_naseApi.GetAstroidById(a.Id.ToString()));
                }
            }

            var newAst = await Task.WhenAll(tasks);
            return newAst;
        }

        private async Task GetNewAsteroidsFromNasa(DateTime startDate, DateTime endDate, List<NearAsteroid> result)
        {
            var astroidDtos = await _naseApi
                .GetClosestAsteroids(startDate, endDate);

            var resultFromNasa = from a in astroidDtos
                                 select a.CopyPropertiesToNew(typeof(NearAsteroid))
                                 as NearAsteroid;

            var inter = result.Select(a => a.Id)
                .Intersect(resultFromNasa.Select(a => a.Id));

            var newAst = from a in resultFromNasa
                         where !inter.Contains(a.Id)
                         select a;
            await InsertMany(newAst);
            result.AddRange(newAst);
        }

        private async Task<bool> IsDbContainCloseApproach(DateTime startDate, DateTime endDate)
        {
            var isContainStart = await MyContext.CloseApproachs
                .AnyAsync(c => c.CloseApproachDate.Date
                == startDate.Date);

            var isContainEnd = await MyContext.CloseApproachs
                 .AnyAsync(c => c.CloseApproachDate.Date
                 == endDate.Date);

            return isContainStart && isContainEnd;
        }

        private async Task<bool> IsDbContainCloseApproachById(DateTime endDate, int astId)
        {
            return await MyContext.CloseApproachs
                .Where(c => c.NearAsteroidId == astId)
                .AnyAsync(c => c.CloseApproachDate.Date > endDate.Date);
        }

        private async Task<bool> IsAsteroidFill(int astId)
        {
            return await MyContext.CloseApproachs
                .Where(c => c.NearAsteroidId == astId)
                .CountAsync() > 1;
        }
    }
}
