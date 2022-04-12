using ApiRequests.Nasa;
using DataAccess.UnitOfWork;
using DomainModel.DbFactory;
using Models;
using Models.Configurations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tools;

namespace DomainModel.Services
{
    public class ImageOfTheDayService : IImageOfTheDayService
    {
        readonly NasaApi _nasaApi;

        readonly IDbFactory _dbFactory;

        public ImageOfTheDayService(IDbFactory dbFactory, MyConfigurations configuration)
        {
            _dbFactory = dbFactory;

            _nasaApi = new NasaApi(configuration.CurrentNasaApiKey);
        }

        public async Task<ImageOfTheDay> GetTodayImage()
        {
            using var unitOfWork = _dbFactory.GetDataAccess();

            var isExist = await
                unitOfWork
                .ImageOfTheDayRepository
                .FindAll(i => i.Date.Date == DateTime.Now.Date);
            if (isExist.Any())
            {
                return isExist.First();
            }

            var imgDto = await _nasaApi.GetImageOfTheDay();

            var result = imgDto.CopyPropertiesToNew(typeof(ImageOfTheDay)) as ImageOfTheDay;

            await unitOfWork
                .ImageOfTheDayRepository
                .Insert(result);

            await unitOfWork.Complete();

            return result;
        }

        public async Task<IEnumerable<ImageOfTheDay>> GetGallery()
        {
            using var unitOfWork = _dbFactory.GetDataAccess();

            var images = await unitOfWork
                .ImageOfTheDayRepository
                .GetAll();
            return images.OrderByDescending(i => i.Date);
        }
    }
}
