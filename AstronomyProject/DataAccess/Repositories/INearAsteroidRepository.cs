using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Models;

namespace DataAccess.Repositories
{
    public interface INearAsteroidRepository : IModelRepository<NearAsteroid>
    {
        Task<IEnumerable<NearAsteroid>> ClosestApproachBetweenDates(DateTime startDate, DateTime endDate = default);
       
        Task<IEnumerable<NearAsteroid>> GetNearAsteroids(Expression<Func<NearAsteroid, bool>> predicate = null);
    }
}
