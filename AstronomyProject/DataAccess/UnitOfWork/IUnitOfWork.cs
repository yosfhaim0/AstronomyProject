using DataAccess.Repositories;
using Models;
using System;
using System.Threading.Tasks;

namespace DataAccess.UnitOfWork
{
    /// <summary>
    /// Unit Of Work
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
        public IModelRepository<ImageOfTheDay> ImageOfTheDayRepository { get; }

        public INearAsteroidRepository NearAstroidRepository { get; }

        public IMediaSearchRepository MediaSearchRepository { get; }

        public IModelRepository<ImaggaTag> ImaggaTagRepository { get; }

        public IModelRepository<SearchWordModel> SearchWordRepository { get; }

        public IModelRepository<CloseApproach> CloseApproachsRepository { get; }

        Task Complete();
    }
}
