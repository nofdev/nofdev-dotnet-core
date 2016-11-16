using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Nofdev.Core.Repository;

namespace Nofdev.Core.Domain
{
    public interface IService<TEntity> where TEntity : class, new()
    {
        /// <summary>
        /// 通过主键获取对象
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        TEntity Get(object id);

        /// <summary>
        /// 增加一个实体
        /// </summary>
        /// <param name="entity"></param>
        void Add(TEntity entity);

        /// <summary>
        /// 批量增加实体
        /// </summary>
        /// <param name="entities"></param>
        void AddRange(IEnumerable<TEntity> entities);

        /// <summary>
        /// 更新一个实体
        /// </summary>
        /// <param name="entity"></param>
        void Update(TEntity entity);

        /// <summary>
        /// 根据主键删除一个实体
        /// </summary>
        /// <param name="id"></param>
        void Delete(object id);

        /// <summary>
        /// 删除一个实体
        /// </summary>
        /// <param name="entity"></param>
        void Delete(TEntity entity);

        /// <summary>
        /// 获取全部对象
        /// </summary>
        /// <returns></returns>
        IQueryable<TEntity> GetQueryable();

        /// <summary>
        /// Fluent API查询
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        IQueryFluent<TEntity> Query(Expression<Func<TEntity, bool>> filter = null);

        /// <summary>
        /// 通过条件判断对象是否存在
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        bool Any(Expression<Func<TEntity, bool>> filter);

        /// <summary>
        /// 统计符合条件的对象总数
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        int Count(Expression<Func<TEntity, bool>> filter);


        /// <summary>
        /// 异步增加一个实体
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task AddAsync(TEntity entity);

        /// <summary>
        /// 异步批量增加一组实体
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        Task AddRangeAsync(IEnumerable<TEntity> entities);

        /// <summary>
        /// 异步更新一个实体
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task UpdateAsync(TEntity entity);

        /// <summary>
        /// 异步删除一个实体
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task DeleteAsync(TEntity entity);

        /// <summary>
        /// 异步获取一个实体
        /// </summary>
        /// <param name="keyValues"></param>
        /// <returns></returns>
        Task<TEntity> GetAsync(params object[] keyValues);

        /// <summary>
        /// 异步删除一个实体
        /// </summary>
        /// <param name="keyValues"></param>
        /// <returns></returns>
        Task<bool> DeleteAsync(params object[] keyValues);

    }

}
