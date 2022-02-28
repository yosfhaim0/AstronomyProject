using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainModel.Services
{
    public class MediaService : IMediaService
    {
        public MediaService()
        {

        }

        public Task<object> SearchMedia(string keyWord)
        {
            return null;
        }
    }
}
