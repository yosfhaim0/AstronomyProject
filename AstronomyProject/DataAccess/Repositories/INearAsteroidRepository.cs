using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Models;

namespace DataAccess.Repositories
{
    public interface INearAsteroidRepository
    {
        Task<IEnumerable<NearAsteroid>> ClosestApproachDateToEarth(DateTime startDate, DateTime endDate = default);

        Task<NearAsteroid> GetFullNearAsteroid(int id);

        Task<IEnumerable<NearAsteroid>> GetNearAsteroids(Expression<Func<NearAsteroid, bool>> predicate = null);
    }
}
