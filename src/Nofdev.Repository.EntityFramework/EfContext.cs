using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Nofdev.Core.Repository;

namespace Nofdev.Repository.EntityFramework
{
    public class EfContext : RepositoryContext, IRepositoryContextAsync, ISqlQuery
    {
        private readonly DbContext _context;
      

        public EfContext(DbContext context)
        {
            _context = context;
        }

        public DbContext DbContext => _context;

        public override void Dispose()
        {
            _context?.Dispose();
        }

        protected override void DoAdd<T>(T entity)
        {
            _context.Set<T>().Attach(entity);
            _context.Entry(entity).State = EntityState.Added;
        }

        protected override void DoUpdate<T>(T entity)
        {
            _context.Set<T>().Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
        }

        protected override void DoDelete<T>(T entity)
        {
            _context.Set<T>().Attach(entity);
            _context.Entry(entity).State = EntityState.Deleted;
        }

        public override int SaveChanges()
        {
           return _context.SaveChanges();
        }


        protected override T DoGet<T>(params object[] keyValues)
        {
           return _context.Find<T>(keyValues);
        }


        protected override IQueryable<T> GetQueryable<T>()
        {
            return _context.Set<T>();
        }


        public Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            return _context.SaveChangesAsync(cancellationToken);
        }

        public Task<int> SaveChangesAsync()
        {
            return _context.SaveChangesAsync();
        }

        public IQueryable<TEntity> SqlQuery<TEntity>(string query, params object[] parameters) where TEntity : class, new()
        {
           return _context.Set<TEntity>().FromSql(query, parameters);
        }

        public int Execute(string sql, params object[] parameters)
        {
           return _context.Database.ExecuteSqlCommand(sql, parameters);
        }

        public Task<int> ExecuteAsync(string sql, params object[] parameters)
        {
            return _context.Database.ExecuteSqlCommandAsync(sql,default(CancellationToken), parameters);
        }
    }
}