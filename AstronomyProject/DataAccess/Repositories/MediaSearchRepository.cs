using ApiRequests.Nasa;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Models;
using System.Threading.Tasks;
using DataAccess.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories
{
    public class MediaSearchRepository : EFModelRepository<MediaGroupe> ,IMediaSearchRepository
    {
        readonly NasaApi _nasaApi;

        public AstronomyContext MyContext { get => Context as AstronomyContext; }

        public MediaSearchRepository(AstronomyContext context ,NasaApi nasaApi) : base(context)
        {
            _nasaApi = nasaApi;
        }

        public async Task<IEnumerable<MediaGroupe>> Search(string searchWord)
        {
            return await MyContext.Media
                .Include(m => m.SearchWords)
                .Where(m => m.SearchWords
                    .Any(s => s.SearchWord == searchWord))
                .ToListAsync();
        }
    }
}
