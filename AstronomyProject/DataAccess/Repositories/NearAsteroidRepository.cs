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

        public AstronomyContext AstronomyContext { get => Context as AstronomyContext; }

        public NearAsteroidRepository(AstronomyContext context, NasaApi nasaApi) : base(context)
        {
            _naseApi = nasaApi;
        }

        public async Task<IEnumerable<NearAsteroid>> GetNearAsteroids(Expression<Func<NearAsteroid, bool>> predicate = null)
        {
            var asteroids = await AstronomyContext
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
                var result = await AstronomyContext.NearAsteroids
                    .Include(a => a.CloseApproachs)
                    .Where(a => a.CloseApproachs.Any(c =>
                     c.CloseApproachDate.Date <= endDate.Date
                      && c.CloseApproachDate.Date >= startDate.Date))
                    .ToListAsync();

                if (!await IsDbContainCloseApproach(startDate, endDate))
                {
                    await GetNewAsteroidsFromNasa(startDate, endDate, result);
                }

                await FillAsteroidsWithCloseApprochData(result, endDate);

                return result;
            }
            catch (Exception)
            {

                throw;
            }
        }

        private async Task FillAsteroidsWithCloseApprochData(List<NearAsteroid> result, DateTime endDate = default)
        {
            var tasks = new List<Task<GetNearAsteroidNasaDto>>();
            foreach (var a in result)
            {
                if((endDate != default && !await IsDbContainCloseApproachById(endDate, a.Id))
                    || !await IsAsteroidFill(a.Id))
                {
                    tasks.Add(_naseApi.GetAstroidById(a.Id.ToString()));
                }
            }
            
            var newAst = await Task.WhenAll(tasks);

            var tasks2 = new List<Task>();
            foreach(var astFromNasa in newAst)
            {
                var ast = astFromNasa.CopyPropertiesToNew(typeof(NearAsteroid)) as NearAsteroid;
                var currAst = result.Find(a => a.Id == ast.Id);
                var isNotFill = !await IsAsteroidFill(currAst.Id);

                var newCA = new List<CloseApproach>();
                if (isNotFill)
                {
                    newCA = ast.CloseApproachs;
                } 
                else if(endDate != default)
                {
                    newCA = (from c in ast.CloseApproachs
                            where c.CloseApproachDate.Date > endDate.Date
                            select c).ToList();
                }
                            
                if (isNotFill)
                    newCA = newCA.Except(currAst.CloseApproachs).ToList();

                if (!newCA.Any())
                {
                    continue;
                }
                
                currAst.CloseApproachs.AddRange(newCA);
                
                tasks2.Add(AstronomyContext.CloseApproachs.AddRangeAsync(newCA));
            }
            await Task.WhenAll(tasks2);
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
            var isContainStart = await AstronomyContext.CloseApproachs
                .AnyAsync(c => c.CloseApproachDate.Date 
                == startDate.Date);

            var isContainEnd = await AstronomyContext.CloseApproachs
                 .AnyAsync(c => c.CloseApproachDate.Date
                 == endDate.Date);

            return isContainStart && isContainEnd;
        }

        private async Task<bool> IsDbContainCloseApproachById(DateTime endDate, int astId)
        {
            return await AstronomyContext.CloseApproachs
                .Where(c => c.NearAsteroidId == astId)
                .AnyAsync(c => c.CloseApproachDate.Date > endDate.Date);
        }

        private async Task<bool> IsAsteroidFill(int astId)
        {
            return await AstronomyContext.CloseApproachs
                .Where(c => c.NearAsteroidId == astId)
                .CountAsync() > 1;
        }
    }
}
