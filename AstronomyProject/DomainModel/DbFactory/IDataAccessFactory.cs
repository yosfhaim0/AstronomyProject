using DataAccess.UnitOfWork;

namespace DomainModel.DataAccessFactory
{
    public interface IDataAccessFactory
    {
        IUnitOfWork GetDataAccess();
    }
}
