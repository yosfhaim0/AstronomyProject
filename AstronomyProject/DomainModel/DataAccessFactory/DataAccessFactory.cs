using DataAccess.UnitOfWork;
using Models.Configurations;

namespace DomainModel.DataAccessFactory
{
    /// <summary>
    /// Data Access Factory using the the Generic Repository & Unit Of Work pattern 
    /// </summary>
    public class DataAccessFactory : IDataAccessFactory
    {
        readonly MyConfigurations _configurations;

        public DataAccessFactory(MyConfigurations configurations)
        {
            _configurations = configurations;
        }

        public IUnitOfWork GetDataAccess()
        {
            return new UnitOfWork(_configurations);
        }
    }
}
