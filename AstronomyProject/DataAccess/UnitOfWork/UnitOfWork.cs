using DataAccess.DbContexts;
using DataAccess.Repositories;
using Microsoft.EntityFrameworkCore;
using Models;
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

        public UnitOfWork(AstronomyContext context)
        {
            _context = context;
            ImageOfTheDayRepository = new EFModelRepository<ImageOfTheDay>(context);
        }

        public IModelRepository<ImageOfTheDay> ImageOfTheDayRepository { get; }

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
