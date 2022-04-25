using System.Collections.Generic;
using System.Linq;
using Models;
using System.Threading.Tasks;
using DataAccess.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories
{
    public class MediaSearchRepository : EFModelRepository<MediaGroupe> ,IMediaSearchRepository
    {
        public AstronomyContext MyContext { get => Context as AstronomyContext; }

        public MediaSearchRepository(AstronomyContext context) : base(context)
        {
        }

        public async Task<IEnumerable<MediaGroupe>> Search(string searchWord)
        {
            var mediaIds = await MyContext.SearchWords
                .Where(s => s.SearchWord == searchWord)
                .Select(s => s.MediaGroupeId)
                .ToListAsync();

            if (!mediaIds.Any())
            {
                return new List<MediaGroupe>();
            }

            return await MyContext.Media
                .Where(m => mediaIds.Contains(m.Id))
                .Include(m => m.Tags)
                .Include(m => m.MediaItems)    
                .ToListAsync();
        }

        public async Task AddSearchWord(MediaGroupe media, SearchWordModel searchWord)
        {
            media.SearchWords.Add(searchWord);
            await MyContext.SearchWords.AddAsync(searchWord);
        }

        public async Task AddTags(MediaGroupe media, IEnumerable<ImaggaTag> tags)
        {
            var mediaFromDB = await GetById(media.Id);
            media.Tags.AddRange(tags);
            mediaFromDB.Tags.AddRange(tags);
            await MyContext.ImaggaTags.AddRangeAsync(tags);
        }
    }
}
