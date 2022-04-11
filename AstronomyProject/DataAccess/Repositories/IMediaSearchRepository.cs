using Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public interface IMediaSearchRepository : IModelRepository<MediaGroupe>
    {
        Task<IEnumerable<MediaGroupe>> Search(string searchWord);

        Task AddSearchWord(MediaGroupe media, SearchWordModel searchWord);

        Task AddTags(MediaGroupe media, IEnumerable<ImaggaTag> tags);
    }
}
