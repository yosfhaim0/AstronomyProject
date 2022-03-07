using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Models;

namespace DomainModel.Services
{
    public interface INearAsteroidService
    {
        Task<IEnumerable<NearAsteroid>> SearchNearAsteroids(DateTime? from, DateTime? to);

        Task<int> CountBy(Expression<Func<NearAsteroid, bool>> predicate);
    }
}
