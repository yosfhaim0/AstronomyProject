using Microsoft.EntityFrameworkCore;
using Models;

namespace DataAccess.DbContexts
{
    /// <summary>
    /// The Database of the project
    /// </summary>
    public class AstronomyContext : DbContext
    {
        public DbSet<ImageOfTheDay> ImageOfTheDayGallery { get; set; }

        public DbSet<NearAsteroid> NearAsteroids { get; set; }

        public DbSet<CloseApproach> CloseApproachs { get; set; }

        public DbSet<MediaGroupe> Media { get; set; }

        public DbSet<MediaItem> MediaItems { get; set; }

        public DbSet<ImaggaTag> ImaggaTags { get; set; }

        public DbSet<SearchWordModel> SearchWords { get; set; }

        public AstronomyContext(DbContextOptions options) : base(options)
        {

        }
    }
}
