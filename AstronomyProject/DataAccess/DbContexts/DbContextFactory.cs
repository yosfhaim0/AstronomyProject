using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DbContexts
{
    public class DbContextFactory
    {
        private readonly string _connctionStrings;

        public DbContextFactory(string connctionStrings)
        {
            _connctionStrings = connctionStrings;
        }

        public AstronomyContext CreateAstronomyContext()
        {
            DbContextOptions options = new DbContextOptionsBuilder()
                .UseSqlServer(_connctionStrings)
                .Options;

            return new AstronomyContext(options);
        }
    }
}
