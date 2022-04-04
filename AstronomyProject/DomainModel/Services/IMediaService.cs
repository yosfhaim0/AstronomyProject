using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainModel.Services
{
    public interface IMediaService
    {
        Task<IEnumerable<MediaGroupe>> SearchMedia(string keyWord); 
    }
}
