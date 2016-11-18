
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Nofdev.MultiTenant.Identity.EntityFramework
{
    /// <summary>
    /// Minimal class for a <see cref="MultiTenantIdentityUserLogin{TKey, TTenant}"/> with a <see cref="string"/> user
    /// <see cref="IdentityUserLogin{TKey}.UserId"/>
    /// and <see cref="MultiTenantIdentityUserLogin{TKey, TTenant}.TenantId"/>.
    /// </summary>
    public class MultiTenantIdentityUserLogin : MultiTenantIdentityUserLogin<string, int>
    {
    }
}
