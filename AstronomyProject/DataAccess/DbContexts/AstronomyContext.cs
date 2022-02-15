using Microsoft.EntityFrameworkCore;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DbContexts
{
    public class AstronomyContext : DbContext
    {
        public AstronomyContext(DbContextOptions dbContextOptions) : base(dbContextOptions)
        {

        }
        public AstronomyContext() : base()
        {

        }
        public DbSet<NASAImageOfTheDay> ImageOfTheDayGallery { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // optionsBuilder.UseSqlServer(@"Data Source=(localdb)\MSSQLLocalDB;Database=EFDemoDB;Trusted_Connection=True;");
        }
    }
}
