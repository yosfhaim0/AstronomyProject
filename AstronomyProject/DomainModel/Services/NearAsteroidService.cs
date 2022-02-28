﻿using DataAccess.UnitOfWork;
using DomainModel.DbFactory;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainModel.Services
{
    public class NearAsteroidService : INearAsteroidService
    {
        readonly IDbFactory _dbFactory;
        readonly IUnitOfWork _unitOfWork;

        public NearAsteroidService(IDbFactory dbFactory)
        {
            _dbFactory = dbFactory;
            _unitOfWork = _dbFactory.GetDataAccess();
        }

        public async Task<IEnumerable<NearAsteroid>> GetPotentiallyHazardous()
        {
            var astroids = await
                _unitOfWork.NearAstroidRepository
                .ClosestApproachDateToEarth(DateTime.Now.AddDays(-3), DateTime.Now);
            return astroids;
        }
    }
}
