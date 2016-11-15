using System;
using System.Collections.Generic;
using System.Data;

namespace Nofdev.Core.Repository
{
    /// <summary>
    /// * 工作单元抽象类（扩展其他工作单元时要继承此类）
    /// </summary>
    public abstract class UnitOfWork : IUnitOfWork
    {
        /// <summary>
        /// * 当前范围内的工作单元对象
        /// </summary>
        [ThreadStatic]
        protected static UnitOfWork _INSTANCE;

        //protected List<ActionDescription> _actions = new List<ActionDescription>();
        /// <summary>
        /// * 当前范围内的工作单元对象
        /// </summary>
        public static UnitOfWork Instance
        {
            get
            {
                return _INSTANCE;
            }
            protected set
            {
                _INSTANCE = value;
            }
        }

        /// <summary>
        /// * 外部工作单元对象
        /// </summary>
        protected UnitOfWork _outSideInstance;

        /// <summary>
        /// * 嵌套的工作单元对象
        /// </summary>
        protected List<UnitOfWork> _innerSideInstance = new List<UnitOfWork>();

   

        /// <summary>
        /// * 嵌套的工作单元对象
        /// </summary>
        public List<UnitOfWork> InnerSideInstance { get { return this._innerSideInstance; } }

        public UnitOfWork()
        {
            this._outSideInstance = UnitOfWork.Instance;

            if (this._outSideInstance != null)
            {
                this._outSideInstance.InnerSideInstance.Add(this);
            }

            UnitOfWork.Instance = this;
        }


        public abstract void BeginTransaction();


        public abstract void Rollback();
        public abstract IRepository<TEntity> GetRepository<TEntity>() where TEntity : class, new();

        /// <summary>
        /// * 提交操作
        /// </summary>
        public abstract void Commit();

        ///// <summary>
        ///// * 操作数据库实现（不包括事务）
        ///// </summary>
        //public void Post()
        //{
        //    while (_actions.Any())
        //        PostLogic();
        //}

        //private void PostLogic()
        //{
        //    var data = _actions.ToArray();
        //    foreach (ActionDescription description in data)
        //    {
        //        if (description.DoBefore != null) { description.DoBefore.Invoke(); }

        //        this.PostContent(description);

        //        if (description.CallBack != null) { description.CallBack.Invoke(); }
        //    }

        //    _actions.RemoveAll(p => data.Any(r => ReferenceEquals(r, p)));
        //}

        ///// <summary>
        ///// * 提交操作的具体实现
        ///// </summary>
        ///// <param name="description">操作描述对象</param>
        //protected abstract void PostContent(ActionDescription description);

        public virtual void Dispose()
        {
            if (this._outSideInstance == null)
            {
                this.DisposeRepository();
            }

            UnitOfWork.Instance = this._outSideInstance;
            this._outSideInstance = null;  // 若为内层UnitOfWork，此时的内层UnitOfWork失去和外层的关系，使该对象可以提交。
        }

        protected void DisposeRepository()
        {
            if (this._innerSideInstance.Count != 0)
            {
                this._innerSideInstance.ForEach(v => v.DisposeRepository());
            }

            //this._repositories.ForEach(v => v.Dispose());
        }

        protected void InnerCommit(UnitOfWork unit)
        {
            foreach (var innerUnit in unit._innerSideInstance)
            {
                innerUnit.Commit();
            }
        }
    }


}
