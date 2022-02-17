using ApiRequests.Nasa;
using DataAccess.DbContexts;
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
        public ImageOfTheDayRepository(AstronomyContext context) : base(context)
        {
        }

        public async Task<ImageOfTheDay> GetImageOfTheDayFromNasa()
        {
            var isExist = await FindAll(i => i.Date.Date == DateTime.Now.Date);
            if (isExist.Any())
            {
                return isExist.First();
            }

            NasaApi nasaApi = new NasaApi();
            var imgDto = await nasaApi.GetImageOfTheDay();

            var result = imgDto.CopyPropertiesToNew(typeof(ImageOfTheDay)) as ImageOfTheDay;

            await Insert(result);

            return result;
        }
    }
}
