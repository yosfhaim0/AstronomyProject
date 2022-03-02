using DataAccess.Repositories;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        public IImageOfTheDayRepository ImageOfTheDayRepository { get; }

        public INearAsteroidRepository NearAstroidRepository { get; }

        public IMediaSearchRepository MediaSearchRepository { get; }
        Task Complete();
    }
}
