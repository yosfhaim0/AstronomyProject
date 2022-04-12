using Microsoft.EntityFrameworkCore;

namespace DataAccess.DbContexts
{
    /// <summary>
    /// Database Context Factory using SqlServer
    /// </summary>
    public class DbContextFactory
    {
        readonly string _connectionStrings;

        public DbContextFactory(string connectionStrings)
        {
            _connectionStrings = connectionStrings;
        }

        public AstronomyContext CreateAstronomyContext()
        {
            var options = new DbContextOptionsBuilder()
                .EnableSensitiveDataLogging()
                .UseSqlServer(_connectionStrings)
                .Options;

            return new AstronomyContext(options);
        }
    }
}
