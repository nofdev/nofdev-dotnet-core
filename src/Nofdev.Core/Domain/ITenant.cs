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

}