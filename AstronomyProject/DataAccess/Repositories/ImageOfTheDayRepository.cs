using ApiRequests.Nasa;
using DataAccess.DbContexts;
using ApiRequests.FireBaseStorage;
using Microsoft.EntityFrameworkCore;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Tools;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public class ImageOfTheDayRepository : EFModelRepository<ImageOfTheDay> ,IImageOfTheDayRepository
    {
        readonly NasaApi _nasaApi;
        readonly FireBase _firebase;

        public ImageOfTheDayRepository(AstronomyContext context, NasaApi nasaApi, FireBase fireBase) : base(context)
        {
            _nasaApi = nasaApi;
            _firebase = fireBase;
        }

        public async Task<ImageOfTheDay> GetImageOfTheDayFromNasa()
        {
            var isExist = await FindAll(i => i.Date.Date == DateTime.Now.Date);
            if (isExist.Any())
            {
                return isExist.First();
            }

            var imgDto = await _nasaApi.GetImageOfTheDay();

            var result = imgDto.CopyPropertiesToNew(typeof(ImageOfTheDay)) as ImageOfTheDay;

            await Insert(result);

            return result;
        }
    }
}
