﻿using DataAccess.UnitOfWork;
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

        public NearAsteroidService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<int> CountBy(Expression<Func<NearAsteroid, bool>> predicate)
        {
            return await _unitOfWork
                .NearAstroidRepository
                .Count(predicate);
        }

        public async Task<IEnumerable<NearAsteroid>> SearchNearAsteroids(DateTime? from, DateTime? to = null)
        {
            if(from == null || to == null)
            {
                throw new ArgumentNullException();
            }

            if ((to.Value - from.Value).Days > 7)
            {
                throw new ArgumentOutOfRangeException("The maximum is 7 days");
            }

            if (to == null)
            {
                to = from.Value.AddDays(7);
            }

            var astroids = await
                _unitOfWork.NearAstroidRepository
                .ClosestApproachBetweenDates(from.Value, to.Value);

            await _unitOfWork.Complete();
            await _unitOfWork.NearAstroidRepository
                .FillAsteroidsWithCloseApprochData(astroids.ToList(), to.Value);
            await _unitOfWork.Complete();

            return astroids;
        }

        public async Task<IEnumerable<NearAsteroid>> GetNearAsteroids(Expression<Func<NearAsteroid, bool>> predicate = null)
        {
            var result = await _unitOfWork
                .NearAstroidRepository.GetNearAsteroids(predicate);
            await _unitOfWork.Complete();
            return result;
        }
    }
}
