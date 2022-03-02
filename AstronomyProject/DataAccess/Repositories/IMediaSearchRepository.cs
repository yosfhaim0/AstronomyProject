using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public interface IMediaSearchRepository
    {
        Task<string> Search(string searchWord);
    }
}
