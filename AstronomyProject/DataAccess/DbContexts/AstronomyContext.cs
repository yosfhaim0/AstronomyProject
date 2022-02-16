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
        public AstronomyContext(DbContextOptions dbContextOptions) : base(dbContextOptions)
        {

        }
        //public AstronomyContext() : base()
        //{

        //}
        public DbSet<ImageOfTheDay> ImageOfTheDayGallery { get; set; }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    var con = @"Data Source=(localdb)\MSSQLLocalDB;Database=AstronomyDB;Trusted_Connection=True;";
        //    optionsBuilder.UseSqlServer(con);
        //}

        //private string GetConnectionString()
        //{
        //    // Equivalent connection string:
        //    // "User Id=<DB_USER>;Password=<DB_PASS>;Server=<DB_HOST>;Database=<DB_NAME>;"
        //    var connectionString = new SqlConnectionStringBuilder()
        //    {
        //        // Remember - storing secrets in plain text is potentially unsafe. Consider using
        //        // something like https://cloud.google.com/secret-manager/docs/overview to help keep
        //        // secrets secret.
        //        DataSource = Environment.GetEnvironmentVariable("35.232.80.145"),     // e.g. '127.0.0.1'
        //        // Set Host to 'cloudsql' when deploying to App Engine Flexible environment
        //        UserID = Environment.GetEnvironmentVariable("sqlserver"),         // e.g. 'my-db-user'
        //        Password = Environment.GetEnvironmentVariable("OJm99mDiDPJycKEC"),       // e.g. 'my-db-password'
        //        InitialCatalog = Environment.GetEnvironmentVariable("astronomy"), // e.g. 'my-database'

        //        // The Cloud SQL proxy provides encryption between the proxy and instance
        //        Encrypt = false,
        //    };
        //    connectionString.Pooling = true;
        //    // ...
        //    return connectionString.ConnectionString;

        //}
    }
}
