using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using LinqKit;
using Nofdev.Core.Domain;
using Nofdev.Core.Repository;
using Microsoft.EntityFrameworkCore;
using LinqKit.Utilities;

namespace Nofdev.Repository.EntityFramework
{
    public class Repository<TEntity> : IRepositoryAsync<TEntity> where TEntity : class,new()
    {
        private readonly IRepositoryContextAsync _context;
        private readonly EfContext _dbContext;
      
        public IUnitOfWorkAsync UoW { get; internal set; }
        public ISqlQuery SqlQuery => _dbContext;
        readonly ConcurrentDictionary<Type,int> _softDeleteTypes = new ConcurrentDictionary<Type,int>();

        public Repository(IRepositoryContextAsync context)
        {
            _context = context;
            _dbContext = context as EfContext;
        }


        public void Add(TEntity entity)
        {
           _context.Add(entity);
            if (UoW == null)
                _context.SaveChanges();
        }

        public Task AddAsync(TEntity entity)
        {
            _context.Add(entity);
            return _context.SaveChangesAsync();
        }


        public void AddRange(IEnumerable<TEntity> entities)
        {
            foreach (var entity in entities)
            {
                _context.Add(entity);
            }
            if (UoW == null)
                _context.SaveChanges();
        }


        public Task AddRangeAsync(IEnumerable<TEntity> entities)
        {
            foreach (var entity in entities)
            {
                _context.Add(entity);
            }
            return _context.SaveChangesAsync();
        }



        protected bool SoftDelete(TEntity entity)
        {
            var soft = MarkSoftDelete(entity);
            if (soft)
            {
                Update(entity);
            }
            return soft;
        }

        protected bool MarkSoftDelete(TEntity entity)
        {
            var type = typeof(TEntity);
            int status;
            if (_softDeleteTypes.TryGetValue(type, out status))
            {
                ((IStateful) entity).Status = status;
                return true;
            }

            var attributes = type.GetTypeInfo().GetCustomAttributes(true);
            foreach (var attribute in attributes)
            {
                var se = attribute as SoftDeleteAttribute;
                if (se != null)
                {
                    status = se.DeleteStatus;

                    _softDeleteTypes.TryAdd(type, status);

                    ((IStateful)entity).Status = status;
                    return true;
                }
            }
            return false;
        }

        public void Delete(params object[] keyValues)
        {
            Delete(Get(keyValues));
        }

        protected void CheckBuiltIn(TEntity entity)
        {
            var builtIn = entity as IHasBuiltIn;
            if (builtIn != null && builtIn.IsBuiltIn)
            {
                throw new UnauthorizedAccessException("The record is system built-in,cann't be deleted.");
            }
        }

        public void Delete(TEntity entity)
        {
            CheckBuiltIn(entity);

            if (SoftDelete(entity)) return;

            _context.Delete(entity);
            if (UoW == null)
                _context.SaveChanges();
        }

        public Task DeleteAsync(TEntity entity)
        {
            CheckBuiltIn(entity);
            if (MarkSoftDelete(entity))
            {
                return UpdateAsync(entity);
            }
            _context.Delete(entity);
            return _context.SaveChangesAsync();
        }


        public TEntity Get(params object[] keyValues)
        {
            var entity = _context.Get<TEntity>(keyValues);
            return entity;
        }



        private Dictionary<string, dynamic> _repositories;

        public IRepository<T> GetRepository<T>() where T : class, new()
        {
            return GetRepositoryAsync<T>();
        }


        public IRepositoryAsync<T> GetRepositoryAsync<T>() where T : class, new()
        {
            if (_repositories == null)
            {
                _repositories = new Dictionary<string, dynamic>();
            }

            var type = typeof(T).Name;

            if (_repositories.ContainsKey(type))
            {
                return (IRepositoryAsync<T>)_repositories[type];
            }

            var repositoryType = typeof(Repository<>);

            _repositories.Add(type, Activator.CreateInstance(repositoryType.MakeGenericType(typeof(TEntity)), _context, this));

            return _repositories[type];
        }

        public IQueryFluent<TEntity> Query(Expression<Func<TEntity, bool>> query = null)
        { 
            var filter = Nofdev.Core.SOA.ServiceContext.Current.User.MakeExpression<TEntity>();
            if (query != null)
                filter = filter.And(query);
            return new QueryFluent<TEntity>(this, filter);
        }



        public IQueryable<TEntity> Queryable()
        {
            return _context.Queryable<TEntity>();
        }


        public  void Update(TEntity entity)
        {
            _context.Update(entity);
            if (UoW == null)
                _context.SaveChanges();
        }


        public Task UpdateAsync(TEntity entity)
        {
            _context.Update(entity);
            return _context.SaveChangesAsync();
        }

        public  async Task<TEntity> GetAsync(params object[] keyValues)
        {
            return await _dbContext.DbContext.Set<TEntity>().FindAsync(keyValues);
        }

        public  async Task<TEntity> GetAsync(CancellationToken cancellationToken, params object[] keyValues)
        {
            return await _dbContext.DbContext.Set<TEntity>().FindAsync(cancellationToken, keyValues);
        }

        public  async Task<bool> DeleteAsync(params object[] keyValues)
        {
            return await DeleteAsync(CancellationToken.None, keyValues);
        }

        public  async Task<bool> DeleteAsync(CancellationToken cancellationToken, params object[] keyValues)
        {
            var entity = await GetAsync(cancellationToken, keyValues);

            if (entity == null)
            {
                return false;
            }

             Delete(entity);

            return true;
        }


        internal IQueryable<TEntity> Select(
       Expression<Func<TEntity, bool>> filter = null,
       Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
       List<Expression<Func<TEntity, object>>> includes = null,
       int? page = null,
       int? pageSize = null)
        {
            IQueryable<TEntity> query = Queryable();

            if (includes != null)
            {
                query = includes.Aggregate(query, (current, include) => current.Include(include));
            }
            if (orderBy != null)
            {
                query = orderBy(query);
            }
            if (filter != null)
            {
                query = query.Where(filter);
            }
            if (page != null && pageSize != null)
            {
                query = query.Skip((page.Value - 1) * pageSize.Value).Take(pageSize.Value);
            }
            return query;
        }

        internal async Task<IEnumerable<TEntity>> SelectAsync(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            List<Expression<Func<TEntity, object>>> includes = null,
            int? page = null,
            int? pageSize = null)
        {
            return await Select(filter, orderBy, includes, page, pageSize).ToListAsync();
        }

    }

}
