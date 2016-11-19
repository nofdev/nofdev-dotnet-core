namespace Nofdev.Core.Domain
{
    /// <summary>
    /// 多租户接口
    /// </summary>
    /// <typeparam name="TTenantKey"></typeparam>
    public interface ITenant<TTenantKey>
    {
        TTenantKey TenantId { get; set; }
    }

    /// <summary>
    /// 默认的多租户接口
    /// </summary>
    public interface ITenant : ITenant<string>
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
    public interface ITenantContext : ITenantContext<string>
    {

    }
}