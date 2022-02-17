﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public interface IModelRepository<TModel> where TModel : class
    {
        Task<IEnumerable<TModel>> GetAll();
        Task<TModel> GetById(int id);
        Task Insert(TModel model);
        Task InsertMany(IEnumerable<TModel> model);
        Task Delete(TModel model);
        Task<IEnumerable<TModel>> FindAll(Expression<Func<TModel, bool>> predicate);


    }
}