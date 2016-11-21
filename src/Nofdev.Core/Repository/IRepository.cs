using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Nofdev.Core.Repository
{
    /// <summary>
    /// 泛型Repository通用接口
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public interface IRepository<TEntity>  where TEntity : class, new()
    {
        /// <summary>
        /// 通过主键获取对象
        /// </summary>
        /// <param name="keyValues"></param>
        /// <returns></returns>
        TEntity Get(params object[] keyValues);

      

        void Add(TEntity entity);
        void AddRange(IEnumerable<TEntity> entities);

        //void AddOrUpdateGraph(TEntity entity);
        //void AddGraphRange(IEnumerable<TEntity> entities);

        /// <summary>
        /// 更新一个实体
        /// </summary>
        /// <param name="entity"></param>
        void Update(TEntity entity);

        /// <summary>
        /// 根据主键删除一个实体
        /// </summary>
        /// <param name="keyValues"></param>
        void Delete(params object[] keyValues);

        /// <summary>
        /// 删除一个实体
        /// </summary>
        /// <param name="entity"></param>
        void Delete(TEntity entity);

        IQueryFluent<TEntity> Query(Expression<Func<TEntity, bool>> query = null);

        IQueryable<TEntity> Queryable();

        IRepository<T> GetRepository<T>() where T : class, new();

        ISqlQuery SqlQuery { get; }

    }

    public interface ISqlQuery
    {
        /// <summary>
        /// 通过SQL语句获取对象
        /// </summary>
        /// <param name="query"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        IQueryable<T> SqlQuery<T>(string query, params object[] parameters) where T : class, new();

        int Execute(string sql, params object[] parameters);

        Task<int> ExecuteAsync(string sql, params object[] parameters);
    }

    public interface IRepositoryAsync<TEntity> : IRepository<TEntity> where TEntity : class, new()
    {
        Task AddAsync(TEntity entity);
        Task AddRangeAsync(IEnumerable<TEntity> entities);
        Task UpdateAsync(TEntity entity);
        Task DeleteAsync(TEntity entity);
        Task<TEntity> GetAsync(params object[] keyValues);
        Task<TEntity> GetAsync(CancellationToken cancellationToken, params object[] keyValues);
        Task<bool> DeleteAsync(params object[] keyValues);
        Task<bool> DeleteAsync(CancellationToken cancellationToken, params object[] keyValues);
        IRepositoryAsync<T> GetRepositoryAsync<T>() where T : class, new();
    }
}