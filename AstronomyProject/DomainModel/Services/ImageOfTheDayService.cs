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
        readonly IUnitOfWork _unitOfWork;

        readonly NasaApi _nasaApi;

        public ImageOfTheDayService(IDbFactory dbFactory, MyConfigurations configuration)
        {
            _unitOfWork = dbFactory.GetDataAccess();

            _nasaApi = new NasaApi(configuration.CurrentNasaApiKey);
        }

        public async Task<ImageOfTheDay> GetTodayImage()
        {
            var isExist = await  
                _unitOfWork
                .ImageOfTheDayRepository
                .FindAll(i => i.Date.Date == DateTime.Now.Date);
            if (isExist.Any())
            {
                return isExist.First();
            }

            var imgDto = await _nasaApi.GetImageOfTheDay();

            var result = imgDto.CopyPropertiesToNew(typeof(ImageOfTheDay)) as ImageOfTheDay;

            await _unitOfWork
                .ImageOfTheDayRepository
                .Insert(result);

            await _unitOfWork.Complete();

            return result;
        }

        public async Task<IEnumerable<ImageOfTheDay>> GetGallery()
        {
            var images = await _unitOfWork
                .ImageOfTheDayRepository
                .GetAll();
            return images.OrderByDescending(i => i.Date);
        }
    }
}
