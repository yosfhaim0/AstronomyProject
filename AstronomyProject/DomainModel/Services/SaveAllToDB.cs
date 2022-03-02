using DataAccess.UnitOfWork;
using DomainModel.DbFactory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainModel.Services
{
    public class SaveAllToDB
    {
        readonly IUnitOfWork _unitOfWork;

        public SaveAllToDB(IDbFactory dbFactory)
        {
            _unitOfWork = dbFactory.GetDataAccess();
        }

        public async Task SaveAsync()
        {
            await _unitOfWork.Complete();
        }
    }
    public interface IImaggaAutoTagingService
    {

    }

    public class ImaggaAutoTagingService : IImaggaAutoTagingService
    {


    }
}
