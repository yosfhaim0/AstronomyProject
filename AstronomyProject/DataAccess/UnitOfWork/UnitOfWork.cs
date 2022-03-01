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
            var dbContexFactory = new DbContextFactory(configurations.CurrentConnectionStrings);
            _context = dbContexFactory.CreateAstronomyContext();

            var nasaApi = new NasaApi(configurations.CurrentNasaApiKey);
            var firebase = new FireBase(configurations.FirebaseConnection);

            ImageOfTheDayRepository = new ImageOfTheDayRepository(_context, nasaApi, firebase);
            NearAstroidRepository = new NearAsteroidRepository(_context, nasaApi);
        }

        public IImageOfTheDayRepository ImageOfTheDayRepository { get; }

        public INearAsteroidRepository NearAstroidRepository { get; }

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
