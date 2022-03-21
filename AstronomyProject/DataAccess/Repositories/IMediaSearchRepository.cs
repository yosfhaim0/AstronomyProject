using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public interface IMediaSearchRepository
    {
        Task<IEnumerable<string>> Search(string searchWord);
    }
}
