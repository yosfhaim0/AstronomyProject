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
        readonly IUnitOfWork _unitOfWork;

        public GalleryImageOfTheDayService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ImageOfTheDay> GetTodayImage()
        {
            var img = await _unitOfWork
                .ImageOfTheDayRepository
                .GetImageOfTheDayFromNasa();
            await _unitOfWork.Complete();
            return img;
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
