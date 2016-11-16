namespace Nofdev.Core.Domain
{
    /// <summary>
    /// 多租户接口
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ITenant<T>
    {
        T TenantId { get; set; }
    }

    /// <summary>
    /// 默认的多租户接口（ID为int)
    /// </summary>
    public interface ITenant : ITenant<int>
    {

    }

    /// <summary>
    /// 租户上下文接口
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ITenantContext<out T>
    {
        T TenantId { get; }
    }

    /// <summary>
    /// 默认的租户上下文接口（ID为int)
    /// </summary>
    public interface ITenantContext : ITenantContext<int>
    {

    }
}