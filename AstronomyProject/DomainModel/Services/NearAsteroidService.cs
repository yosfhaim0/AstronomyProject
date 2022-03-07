using DataAccess.UnitOfWork;
using DomainModel.DbFactory;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DomainModel.Services
{
    public class NearAsteroidService : INearAsteroidService
    {
        readonly IUnitOfWork _unitOfWork;

        public NearAsteroidService(IDbFactory dbFactory)
        {
            _unitOfWork = dbFactory.GetDataAccess();
        }

        public async Task<int> CountBy(Expression<Func<NearAsteroid, bool>> predicate)
        {
            return await _unitOfWork
                .NearAstroidRepository
                .Count(predicate);
        }

        public async Task<IEnumerable<NearAsteroid>> SearchNearAsteroids(DateTime? from, DateTime? to)
        {
            if(from == null || to == null)
            {
                throw new ArgumentNullException();
            }

            if ((to.Value - from.Value).Days > 7)
            {
                throw new ArgumentOutOfRangeException("The maximum is 7 days");
            }

            var astroids = await
                _unitOfWork.NearAstroidRepository
                .ClosestApproachDateToEarth(from.Value, to.Value);
            
            return astroids;
        }
    }
}
