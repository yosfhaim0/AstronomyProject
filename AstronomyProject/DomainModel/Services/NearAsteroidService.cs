using ApiRequests.Nasa;
using DomainModel.DataAccessFactory;
using Models;
using Models.Configurations;
using Models.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Tools;

namespace DomainModel.Services
{
    public class NearAsteroidService : INearAsteroidService
    {
        readonly NasaApi _nasaApi;

        readonly IDataAccessFactory _daFactory;

        public NearAsteroidService(IDataAccessFactory daFactory, MyConfigurations configuration)
        {
            _daFactory = daFactory;

            _nasaApi = new NasaApi(configuration.CurrentNasaApiKey);
        }

        public async Task<int> CountBy(Expression<Func<NearAsteroid, bool>> predicate)
        {
            using var unitOfWork = _daFactory.GetDataAccess();

            return await unitOfWork
                .NearAstroidRepository
                .Count(predicate);
        }

        public async Task<IEnumerable<NearAsteroid>> SearchNearAsteroids(DateTime? from, DateTime? to = null)
        {
            if(from == null || to == null)
            {
                throw new ArgumentNullException();
            }

            if (to == null)
            {
                to = from.Value.AddDays(7);
            }

            using var unitOfWork = _daFactory.GetDataAccess();

            var astroids = await
                unitOfWork.NearAstroidRepository
                .ClosestApproachBetweenDates(from.Value, to.Value);

            if (!astroids.Any() || !await IsDbContainCloseApproach(from.Value, to.Value))
            {
                await GetNewAsteroidsFromNasa(from.Value, to.Value, astroids);
            }

            await FillAsteroidsWithCloseApprochData(astroids, to.Value);

            await unitOfWork.Complete();

            return astroids;
        }

        public async Task<IEnumerable<NearAsteroid>> GetNearAsteroids(Expression<Func<NearAsteroid, bool>> predicate = null)
        {
            using var unitOfWork = _daFactory.GetDataAccess();

            var result = await unitOfWork
                .NearAstroidRepository.GetNearAsteroids(predicate);

            await FillAsteroidsWithCloseApprochData(result);

            await unitOfWork.Complete();

            return result;
        }

        private async Task GetNewAsteroidsFromNasa(DateTime startDate, DateTime endDate, IEnumerable<NearAsteroid> result)
        {
            if ((endDate - startDate).Days > 7)
            {
                throw new ArgumentOutOfRangeException("The maximum is 7 days");
            }
            var astroidDtos = await _nasaApi
                .GetClosestAsteroids(startDate, endDate);

            var resultFromNasa = from a in astroidDtos
                                 select a.CopyPropertiesToNew(typeof(NearAsteroid))
                                 as NearAsteroid;

            var inter = result.Select(a => a.Id)
                .Intersect(resultFromNasa.Select(a => a.Id));

            var newAst = from a in resultFromNasa
                         where !inter.Contains(a.Id)
                         select a;

            using var unitOfWork = _daFactory.GetDataAccess();

            await unitOfWork
                .NearAstroidRepository
                .InsertMany(newAst);
            
            await unitOfWork.Complete(); 
            result.ToList().AddRange(newAst);
        }

        private async Task<GetNearAsteroidNasaDto[]> GetCloseApprochDataFromNasa(IEnumerable<NearAsteroid> asteroids, DateTime endDate)
        {
            var tasks = new List<Task<GetNearAsteroidNasaDto>>();
            foreach (var a in asteroids)
            {
                bool isDbContainCloseApproach = endDate != default && await IsDbContainCloseApproachById(endDate, a.Id);
                bool isAsteroidFill = await IsAsteroidFill(a.Id);
                if (!isDbContainCloseApproach || !isAsteroidFill)
                {
                    tasks.Add(_nasaApi.GetAstroidById(a.Id.ToString()));
                }
            }

            var newAst = await Task.WhenAll(tasks);
            return newAst;
        }

        private async Task<bool> IsDbContainCloseApproach(DateTime startDate, DateTime endDate)
        {
            using var unitOfWork = _daFactory.GetDataAccess();

            var isContainStart = await unitOfWork.CloseApproachsRepository
                .Any(c => c.CloseApproachDate.Date
                == startDate.Date);

            var isContainEnd = await unitOfWork.CloseApproachsRepository
                .Any(c => c.CloseApproachDate.Date
                 == endDate.Date);

            return isContainStart && isContainEnd;
        }

        private async Task<bool> IsDbContainCloseApproachById(DateTime endDate, int astId)
        {
            using var unitOfWork = _daFactory.GetDataAccess();

            return await unitOfWork.CloseApproachsRepository
                .Any(c => c.NearAsteroidId == astId &&
                c.CloseApproachDate.Date > endDate.Date);
        }

        private async Task<bool> IsAsteroidFill(int astId)
        {
            using var unitOfWork = _daFactory.GetDataAccess();

            return await unitOfWork.CloseApproachsRepository
                .Any(c => c.NearAsteroidId == astId);
        }

        private async Task FillAsteroidsWithCloseApprochData(IEnumerable<NearAsteroid> asteroids, DateTime endDate = default)
        {
            var newAstroidsFromNasa = await GetCloseApprochDataFromNasa(asteroids, endDate);

            await InsertNewCloseApprochsToDb(asteroids, endDate, newAstroidsFromNasa);
        }

        private async Task InsertNewCloseApprochsToDb(IEnumerable<NearAsteroid> asteroids, DateTime endDate, GetNearAsteroidNasaDto[] newAstroidsFromNasa)
        {
            var tasks = new List<Task>();

            using var unitOfWork = _daFactory.GetDataAccess();

            foreach (var astFromNasa in newAstroidsFromNasa)
            {
                var currAstFromDb = await unitOfWork
                    .NearAstroidRepository
                    .GetById(astFromNasa.Id);
                var currNewAst = asteroids.FirstOrDefault(a => a.Id == astFromNasa.Id);
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

                tasks.Add(unitOfWork
                    .CloseApproachsRepository
                    .InsertMany(newCA));
            }
            await Task.WhenAll(tasks);
            await unitOfWork.Complete();
        }
    }
}
