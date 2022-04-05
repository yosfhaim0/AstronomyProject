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
            var mediaIds = await MyContext.SearchWords
                .Where(s => s.SearchWord == searchWord)
                .Select(s => s.MediaGroupeId)
                .ToListAsync();

            return await MyContext.Media
                .Where(m => mediaIds.Contains(m.Id))
                .Include(m => m.Tags)
                .Include(m => m.MediaItems)    
                .ToListAsync();
        }

        
    }
}
