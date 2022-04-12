using ApiRequests.Nasa;
using DataAccess.DbContexts;
using Microsoft.EntityFrameworkCore;
using Models;
using Models.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Tools;

namespace DataAccess.Repositories
{
    public class NearAsteroidRepository : EFModelRepository<NearAsteroid>, INearAsteroidRepository
    {
        public AstronomyContext MyContext { get => Context as AstronomyContext; }

        public NearAsteroidRepository(AstronomyContext context) : base(context)
        {
        }

        public async Task<IEnumerable<NearAsteroid>> GetNearAsteroids(Expression<Func<NearAsteroid, bool>> predicate = null)
        {
            var asteroids = await MyContext
                .NearAsteroids
                .AsNoTracking()
                .Include(a => a.CloseApproachs)
                .Where(predicate is null ? _ => true : predicate)
                .ToListAsync();
            return asteroids;
        }

        public async Task<IEnumerable<NearAsteroid>> ClosestApproachBetweenDates(DateTime startDate, DateTime endDate = default)
        {
            try
            {
                var asteroids = await MyContext.NearAsteroids
                    .AsNoTracking()
                     .Include(a => a.CloseApproachs)
                     .Where(a => a.CloseApproachs.Any(c =>
                      c.CloseApproachDate.Date <= endDate.Date
                       && c.CloseApproachDate.Date >= startDate.Date))
                     .ToListAsync();
                return asteroids;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
