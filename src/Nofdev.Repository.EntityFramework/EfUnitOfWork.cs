using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Storage;
using Nofdev.Core.Repository;

namespace Nofdev.Repository.EntityFramework
{
    public class EfUnitOfWork : UnitOfWork, IUnitOfWorkAsync
    {
        private readonly IRepositoryContextAsync _context;
        private readonly EfContext _efContext;
      
        private Dictionary<string, dynamic> _repositories;
        private IDbContextTransaction _transaction;

        public EfUnitOfWork(IRepositoryContextAsync context)
        {
            _context = context;
            _efContext = _context as EfContext;
        }

        public override void BeginTransaction()
        {
           _transaction =  _efContext.DbContext.Database.BeginTransaction();
        }

        public override IRepository<TEntity> GetRepository<TEntity>()
        {
            return GetRepositoryAsync<TEntity>();
        }

        public override void Commit()
        {
            if (_transaction != null)
                _transaction.Commit();
            else
                _context.SaveChanges();
        }


        public Task CommitAsync()
        {
            if (_transaction != null)
                return new TaskFactory().StartNew(() => _transaction.Commit());
            else
                return _context.SaveChangesAsync();
        }

        public IRepositoryAsync<T> GetRepositoryAsync<T>() where T : class, new()
        {
            if (_repositories == null)
            {
                _repositories = new Dictionary<string, dynamic>();
            }

            var type = typeof (T).Name;

            if (_repositories.ContainsKey(type))
            {
                return (IRepositoryAsync<T>) _repositories[type];
            }

            var repositoryType = typeof (Repository<>);

            var instance = Activator.CreateInstance(repositoryType.MakeGenericType(typeof (T)), _context);
            (instance as Repository<T>).UoW = this;
            _repositories.Add(type, instance);

            return _repositories[type];
        }

        public override void Rollback()
        {
            _transaction.Rollback();
        }


        public Task<int> SaveChangesAsync()
        {
            return _context.SaveChangesAsync();
        }

        public Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            return _context.SaveChangesAsync(cancellationToken);
        }
    }

    public class UowFactory : IUowFactory
    {
        #region Implementation of IUowFactory

        public IUnitOfWorkAsync Start(IRepositoryContextAsync context)
        {
            return new EfUnitOfWork(context);
        }

        #endregion
    }
}