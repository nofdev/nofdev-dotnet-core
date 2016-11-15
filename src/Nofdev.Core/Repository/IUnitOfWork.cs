using System;
using System.Data;
using System.Threading.Tasks;

namespace Nofdev.Core.Repository
{
    /// <summary>
    /// 工作单元接口
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
        void BeginTransaction();
        void Commit();
        void Rollback();

        IRepository<TEntity> GetRepository<TEntity>() where TEntity : class, new();
    }

    public interface IUnitOfWorkAsync : IUnitOfWork
    {
        Task CommitAsync();
        //Task RollbackAsync();
        //Task<int> SaveChangesAsync();
        //Task<int> SaveChangesAsync(CancellationToken cancellationToken);
        IRepositoryAsync<TEntity> GetRepositoryAsync<TEntity>() where TEntity : class, new();
    }

    public interface IUowFactory
    {
        IUnitOfWorkAsync Start(IRepositoryContextAsync context);
    }
}
