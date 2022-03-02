using DataAccess.UnitOfWork;
using DomainModel.DbFactory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainModel.Services
{
    public class MediaService : IMediaService
    {
        readonly IUnitOfWork _unitOfWork;
        IImaggaAutoTagingService _imaggaAutoTaging;

        public MediaService(IDbFactory dbFactory, IImaggaAutoTagingService imaggaAutoTaging)
        {
            _unitOfWork = dbFactory.GetDataAccess();
            _imaggaAutoTaging = imaggaAutoTaging;

        }


        public async Task<List<string>> SearchMedia(string keyWord)
        {
            var imgs = await _unitOfWork.MediaSearchRepository.Search(keyWord);
            return RootToString(imgs);
        }

        private List<string> RootToString(Models.Dtos.Root imgs)
        {
            var a = imgs.collection.items;
            List<string> result = new List<string>();
            foreach (var item in a)
            {
                if (item.links != null)
                    foreach (var link in item.links)
                    {
                        if (link != null)
                            result.Add(link.href);
                    }
            }
            return result;
        }
    }
}
