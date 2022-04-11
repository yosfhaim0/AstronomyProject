using DataAccess.UnitOfWork;
using Models.Configurations;

namespace DomainModel.DbFactory
{
    public class DbFactory : IDbFactory
    {
        readonly MyConfigurations _configurations;

        public DbFactory(MyConfigurations configurations)
        {
            _configurations = configurations;
        }

        public IUnitOfWork GetDataAccess()
        {
            return new UnitOfWork(_configurations);
        }
    }
}
