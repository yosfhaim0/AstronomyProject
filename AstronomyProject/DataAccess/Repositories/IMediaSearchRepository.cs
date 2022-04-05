using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public interface IMediaSearchRepository : IModelRepository<MediaGroupe>
    {
        Task<IEnumerable<MediaGroupe>> Search(string searchWord);

        Task AddSearchWord(MediaGroupe media, SearchWordModel searchWord);
        Task AddTags(MediaGroupe media, List<ImaggaTag> tags);
    }
}
