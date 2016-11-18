
namespace Nofdev.MultiTenant.Identity.EntityFramework
{
    /// <summary>
    /// Minimal interface for a <see cref="IMultiTenantUser{TKey, TTenant}"/> with a <see cref="string"/> user
    /// <see cref="IUser{TKey}.Id"/> and <see cref="IMultiTenantUser{TKey,TTenant}.TenantId"/>.
    /// </summary>
    public interface IMultiTenantUser : IMultiTenantUser<string>
    {
    }
}
