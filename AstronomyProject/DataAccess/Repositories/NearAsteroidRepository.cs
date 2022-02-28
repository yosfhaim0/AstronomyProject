using ApiRequests.Nasa;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Tools;

namespace DataAccess.Repositories
{
    public class NearAsteroidRepository : INearAsteroidRepository
    {
        readonly NasaApi _naseApi = new();

        public async Task<IEnumerable<NearAsteroid>> ClosestApproachDateToEarth(DateTime startDate, DateTime endDate = default)
        {
            if (endDate == default)
            {
                endDate = startDate.AddDays(7);
            }

            try
            {
                var astroidDtos = await _naseApi.GetClosestAsteroids(startDate, endDate);

                var result = from a in astroidDtos
                             select a.CopyPropertiesToNew(typeof(NearAsteroid))
                             as NearAsteroid;

                return result;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<NearAsteroid> GetFullNearAsteroid(int id)
        {
            try
            {
                var asteroid = await _naseApi.GetAstroidById(id.ToString());
                return asteroid.CopyPropertiesToNew(typeof(NearAsteroid)) as NearAsteroid;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public Task<IEnumerable<NearAsteroid>> GetNearAsteroids(Expression<Func<NearAsteroid, bool>> predicate = null)
        {
            throw new NotImplementedException();
        }
    }
}
