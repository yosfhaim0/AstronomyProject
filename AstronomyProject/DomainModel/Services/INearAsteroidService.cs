using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models;

namespace DomainModel.Services
{
    public interface INearAsteroidService
    {
        Task<IEnumerable<NearAsteroid>> GetPotentiallyHazardous();


    }
}
