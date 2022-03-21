using Microsoft.EntityFrameworkCore;
using Models.Configurations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DbContexts
{
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
                .UseSqlServer(_connectionStrings)
                .Options;

            return new AstronomyContext(options);
        }
    }
}
