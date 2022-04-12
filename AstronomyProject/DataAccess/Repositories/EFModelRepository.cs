using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    /// <summary>
    /// Implementaion of Generic Repository using Entity Framework ORM
    /// </summary>
    /// <typeparam name="TModel"></typeparam>
    public class EFModelRepository<TModel> : IModelRepository<TModel> where TModel : class
    {
        readonly protected DbContext Context;

        public EFModelRepository(DbContext dbContext)
        {
            Context = dbContext;
        }

        public async Task Delete(TModel model)
        {
            await Task.Run(() => Context.Set<TModel>().Remove(model));
        }

        public async Task<IEnumerable<TModel>> FindAll(Expression<Func<TModel, bool>> predicate)
        {
            return await Context.Set<TModel>().Where(predicate).ToListAsync();
        }

        public async Task<IEnumerable<TModel>> GetAll()
        {
            return await Context.Set<TModel>().ToListAsync();
        }

        public async Task<TModel> GetById(int id)
        {
            return await Context.Set<TModel>().FindAsync(id);
        }

        public async Task Insert(TModel model)
        {
            await Context.Set<TModel>().AddAsync(model);
        }

        public async Task InsertMany(IEnumerable<TModel> models)
        {
            await Context.Set<TModel>().AddRangeAsync(models);
        }

        public async Task<int> Count(Expression<Func<TModel, bool>> predicate = null)
        {
            if(predicate == null)
            {
                return await Context.Set<TModel>().CountAsync();
            }
            return await Context.Set<TModel>().CountAsync(predicate);
        }

        public async Task<bool> Any(Expression<Func<TModel, bool>> predicate = null)
        {
            if (predicate == null)
            {
                return await Context.Set<TModel>().AnyAsync();
            }
            return await Context.Set<TModel>().AnyAsync(predicate);
        }

        public async Task<bool> All(Expression<Func<TModel, bool>> predicate)
        {
            return await Context.Set<TModel>().AllAsync(predicate);
        }
    }
}
