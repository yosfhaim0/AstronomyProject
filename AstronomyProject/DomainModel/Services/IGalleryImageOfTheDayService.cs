using DataAccess.UnitOfWork;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainModel.Services
{
    public interface IGalleryImageOfTheDayService
    {
        Task<ImageOfTheDay> GetTodayImage();

        Task<IEnumerable<ImageOfTheDay>> GetGallery();
    }
}
