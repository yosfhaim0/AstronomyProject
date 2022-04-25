using Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DomainModel.Services
{
    public interface IMediaService
    {
        Task<IEnumerable<ImaggaTag>> GetMediaTags(MediaGroupe media);
        Task<IEnumerable<MediaGroupe>> SearchMedia(string keyWord);
        Task<IEnumerable<MediaGroupe>> SearchMedia(string keyWord, int skip);
        Task<IEnumerable<string>> GetSearchWords();
    }
}
