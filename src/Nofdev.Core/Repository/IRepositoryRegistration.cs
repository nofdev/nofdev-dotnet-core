using System;

namespace Nofdev.Core.Repository
{
    /// <summary>
    /// 仓储接口
    /// </summary>
    public interface IRepositoryRegistration : IDisposable
    {
        void Add<T>(T entity) where T : class, new();
        void Update<T>(T entity) where T : class, new();
        void Delete<T>(T entity) where T : class, new();
    }


}