
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Nofdev.Multitenancy.Identity.EntityFramework
{
    /// <summary>
    /// Minimal class for a <see cref="MultitenancyIdentityUserLogin{TKey, TTenant}"/> with a <see cref="string"/> user
    /// <see cref="IdentityUserLogin{TKey}.UserId"/>
    /// and <see cref="MultitenancyIdentityUserLogin{TKey, TTenant}.TenantId"/>.
    /// </summary>
    public class MultitenancyIdentityUserLogin : MultitenancyIdentityUserLogin<string, int>
    {
    }
}
