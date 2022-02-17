using DataAccess.UnitOfWork;
using Models.Configurations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainModel.DbFactory
{
    public interface IDbFactory
    {
        IUnitOfWork GetDataAccess();
    }
}
