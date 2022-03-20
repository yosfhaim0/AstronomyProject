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
        public DbContextFactory(MyConfigurations configurations)
        {
            Options = new DbContextOptionsBuilder()
                .UseSqlServer(configurations.CurrentConnectionStrings)
                .Options;
        }

        public DbContextOptions Options { get; init; }
    }
}
