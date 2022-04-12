using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Models;

namespace DomainModel.Services
{
    /// <summary>
    /// Near Asteroid service
    /// </summary>
    public interface INearAsteroidService
    {
        Task<IEnumerable<NearAsteroid>> GetNearAsteroids(Expression<Func<NearAsteroid, bool>> predicate = null);

        Task<IEnumerable<NearAsteroid>> SearchNearAsteroids(DateTime? from, DateTime? to);

        Task<int> CountBy(Expression<Func<NearAsteroid, bool>> predicate);
    }
}
