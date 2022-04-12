using ApiRequests.FireBaseStorage;
using DataAccess.DbContexts;
using DataAccess.Repositories;
using Models;
using Models.Configurations;
using System.Threading.Tasks;

namespace DataAccess.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        readonly AstronomyContext _context;

        public UnitOfWork(MyConfigurations configurations)
        {
            _context = new DbContextFactory(configurations.CurrentConnectionStrings)
                .CreateAstronomyContext();

            var firebase = new FireBase(configurations.FirebaseConnection);

            NearAstroidRepository = new NearAsteroidRepository(_context);
            MediaSearchRepository = new MediaSearchRepository(_context);

            ImageOfTheDayRepository = new EFModelRepository<ImageOfTheDay>(_context);
            ImaggaTagRepository = new EFModelRepository<ImaggaTag>(_context);
            SearchWordRepository = new EFModelRepository<SearchWordModel>(_context);
            CloseApproachsRepository = new EFModelRepository<CloseApproach>(_context);
        }

        public IModelRepository<ImaggaTag> ImaggaTagRepository { get; }

        public IModelRepository<ImageOfTheDay> ImageOfTheDayRepository { get; }

        public INearAsteroidRepository NearAstroidRepository { get; }

        public IMediaSearchRepository MediaSearchRepository { get; }

        public IModelRepository<SearchWordModel> SearchWordRepository { get; }

        public IModelRepository<CloseApproach> CloseApproachsRepository { get; }

        public async Task Complete()
        {
            await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        
    }
}
