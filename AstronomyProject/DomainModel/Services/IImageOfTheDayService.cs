using Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DomainModel.Services
{
    public interface IImageOfTheDayService
    {
        Task<ImageOfTheDay> GetTodayImage();

        Task<IEnumerable<ImageOfTheDay>> GetGallery();
    }
}
