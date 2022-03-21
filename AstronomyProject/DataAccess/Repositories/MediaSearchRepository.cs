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

        public async Task<IEnumerable<string>> Search(string searchWord)
        {
            return await _nasaApi.SearchImage(searchWord);
            // get from nasa
            // get from db
            // get from firbase
        }
    }
}
