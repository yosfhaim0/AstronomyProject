using DataAccess.Repositories;
using Models;
using System;
using System.Threading.Tasks;

namespace DataAccess.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        public IModelRepository<ImageOfTheDay> ImageOfTheDayRepository { get; }

        public INearAsteroidRepository NearAstroidRepository { get; }

        public IMediaSearchRepository MediaSearchRepository { get; }

        public IModelRepository<ImaggaTag> ImaggaTagRepository { get; }

        public IModelRepository<SearchWordModel> SearchWordRepository { get; }

        Task Complete();
    }
}
