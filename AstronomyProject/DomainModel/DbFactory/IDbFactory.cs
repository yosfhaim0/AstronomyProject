using DataAccess.UnitOfWork;

namespace DomainModel.DbFactory
{
    public interface IDbFactory
    {
        IUnitOfWork GetDataAccess();
    }
}
