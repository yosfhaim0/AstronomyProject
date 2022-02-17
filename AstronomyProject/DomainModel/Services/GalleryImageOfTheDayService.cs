using DataAccess.UnitOfWork;
using DomainModel.DbFactory;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainModel.Services
{
    public class GalleryImageOfTheDayService : IGalleryImageOfTheDayService
    {
        readonly IDbFactory _dbFactory;
        readonly IUnitOfWork _unitOfWork;

        public GalleryImageOfTheDayService(IDbFactory dbFactory)
        {
            _dbFactory = dbFactory;
            _unitOfWork = _dbFactory.GetDataAccess();
        }

        public async Task<ImageOfTheDay> GetTodayImage()
        {
            var img = await _unitOfWork
                .ImageOfTheDayRepository
                .GetImageOfTheDayFromNasa();
            await _unitOfWork.Complete();
            return img;
        }

        public Task<IEnumerable<ImageOfTheDay>> GetGallery()
        {
            throw new NotImplementedException();
        }
    }
}
