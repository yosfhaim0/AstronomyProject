using ApiRequests.FireBaseStorage;
using ApiRequests.Nasa;
using DataAccess.DbContexts;
using DataAccess.Repositories;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Models;
using Models.Configurations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

            var nasaApi = new NasaApi(configurations.CurrentNasaApiKey);
            var firebase = new FireBase(configurations.FirebaseConnection);

            ImageOfTheDayRepository = new ImageOfTheDayRepository(_context, nasaApi, firebase);
            NearAstroidRepository = new NearAsteroidRepository(_context, nasaApi);
            MediaSearchRepository = new MediaSearchRepository(_context ,nasaApi);
        }

        public IImageOfTheDayRepository ImageOfTheDayRepository { get; }

        public INearAsteroidRepository NearAstroidRepository { get; }

        public IMediaSearchRepository MediaSearchRepository { get; }

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
