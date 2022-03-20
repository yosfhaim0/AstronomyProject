using Microsoft.EntityFrameworkCore;
using Models;

namespace DataAccess.DbContexts
{
    public interface IAstronomyContext
    {
        DbSet<CloseApproach> CloseApproachs { get; set; }
        DbSet<ImageOfTheDay> ImageOfTheDayGallery { get; set; }
        DbSet<NearAsteroid> NearAsteroids { get; set; }
    }
}