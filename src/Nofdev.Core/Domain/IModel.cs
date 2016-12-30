using System;

namespace Nofdev.Core.Domain
{
    /// <summary>
    /// common entity interface
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IEntity<T>
    {
        T Id { get; set; }
    }

    public interface IEntity : IEntity<string>
    {
        
    }

    /// <summary>
    /// commmon immutable model interface
    /// </summary>
    public interface IImmutableModel<T,TUser> : IEntity<T>
    {
        /// <summary>
        ///   created datetime
        /// </summary>
         DateTime CreatedDate { get; set; }

        /// <summary>
        ///     created by
        /// </summary>
         TUser CreatedBy { get; set; }
    }

    /// <summary>
    /// default immutable model with String type ID
    /// </summary>
    public interface IImmutableModel : IImmutableModel<string,string>
    {
        
    }

    public abstract class ImmutableModel<T,TUser> : IImmutableModel<T,TUser>
    {
        public virtual T Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public TUser CreatedBy { get; set; }
    }

    /// <summary>
    /// mutable model interface
    /// </summary>
    public interface IMutableModel<T, TUser> : IImmutableModel<T, TUser>
    {
        /// <summary>
        ///     updated by 
        /// </summary>
        TUser UpdatedBy { get; set; }

        /// <summary>
        ///    updated datetime
        /// </summary>
        DateTime? UpdatedDate { get; set; }
    }

    /// <summary>
    /// default mutable model  with String type ID
    /// </summary>
    public interface IMutableModel : IMutableModel<string,string>
    {

    }

    public abstract class MutableModel<T, TUser> : ImmutableModel<T, TUser>
    {
        public TUser UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }


    /// <summary>
    /// stateful model interface
    /// </summary>
    public interface IStatefulModel<T, TUser> : IMutableModel<T, TUser>, IStateful
    {
    }

    /// <summary>
    /// default stateful model interface with String type ID
    /// </summary>
    public interface IStatefulModel  : IStatefulModel<string,string>
    {

    }

    public abstract class StatefulModel<T, TUser> : MutableModel<T, TUser>, IStatefulModel<T, TUser>
    {
        public int Status { get; set; }
    }

    public abstract class BaseEntity<T, TUser> : StatefulModel<T, TUser>
    {
        public string Code { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }
    }

    public abstract class BaseEntity : BaseEntity<int,string>
    {
        
    }
}
