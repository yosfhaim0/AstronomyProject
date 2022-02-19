using Microsoft.Data.SqlClient;
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

        public DbSet<ImageOfTheDay> ImageOfTheDayGallery { get; set; }

        public AstronomyContext(DbContextOptions dbContextOptions) : base(dbContextOptions)
        {

        }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    var con = @"Data Source=(localdb)\MSSQLLocalDB;Database=AstronomyDB;Trusted_Connection=True;";
        //    optionsBuilder.UseSqlServer(con);
        //}

        //public AstronomyContext() : base()
        //{

        //}

    }
}
