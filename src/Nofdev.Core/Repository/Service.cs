using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Nofdev.Core.Domain;

namespace Nofdev.Core.Repository
{

    public class Service<TEntity> : IService<TEntity> where TEntity : class, new()
    {
        protected IRepositoryAsync<TEntity> Repository;
        public Service(IRepositoryAsync<TEntity> repository)
        {
            Repository = repository;
        }

        public TEntity Get(object id)
        {
            return Repository.Get(id);
        }


        public void Add(TEntity entity)
        {
            Repository.Add(entity);
        }

        public void AddRange(IEnumerable<TEntity> entities)
        {
            Repository.AddRange(entities);
        }

        public void Update(TEntity entity)
        {
            Repository.Update(entity);
        }

        public void Delete(object id)
        {
            Repository.Delete(id);
        }

        public void Delete(TEntity entity)
        {
            Repository.Update(entity);
        }

        public IQueryable<TEntity> GetQueryable()
        {
            return Repository.Queryable();
        }


        public bool Any(Expression<Func<TEntity, bool>> filter)
        {
            return GetQueryable().Any(filter.Compile());
        }

        public int Count(Expression<Func<TEntity, bool>> filter)
        {
            return GetQueryable().Count(filter.Compile());
        }

        public IQueryFluent<TEntity> Query(Expression<Func<TEntity, bool>> filter = null)
        {
            return Repository.Query(filter);
        }

        public Task AddAsync(TEntity entity)
        {
            return Repository.AddAsync(entity);
        }

        public Task AddRangeAsync(IEnumerable<TEntity> entities)
        {
            return Repository.AddRangeAsync(entities);
        }

        public Task UpdateAsync(TEntity entity)
        {
            return Repository.UpdateAsync(entity);
        }

        public Task DeleteAsync(TEntity entity)
        {
            return Repository.DeleteAsync(entity);
        }

        public Task<TEntity> GetAsync(params object[] keyValues)
        {
            return Repository.GetAsync(keyValues);
        }

        public Task<bool> DeleteAsync(params object[] keyValues)
        {
            return Repository.DeleteAsync(keyValues);
        }
    }
}
