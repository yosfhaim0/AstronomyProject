using ApiRequests.Nasa;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public class MediaSearchRepository : IMediaSearchRepository
    {
        readonly NasaApi _nasaApi;
        public MediaSearchRepository(NasaApi nasaApi)
        {
            _nasaApi = nasaApi;
        }

        public async Task<Models.Dtos.Root> Search(string searchWord)
        {
            return await _nasaApi.SearchImage(searchWord);
        }
    }
}
