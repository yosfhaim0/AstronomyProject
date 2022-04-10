﻿using DataAccess.UnitOfWork;
using Models.Configurations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainModel.DbFactory
{
    public class DbFactory : IDbFactory
    {
        readonly IUnitOfWork _unitOfWork;
        readonly MyConfigurations _configurations;

        public DbFactory(MyConfigurations configurations)
        {
            _unitOfWork = new UnitOfWork(configurations);
            _configurations = configurations;
        }

        public IUnitOfWork GetDataAccess()
        {
            return new UnitOfWork(_configurations);
        }
    }
}
