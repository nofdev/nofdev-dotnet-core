
using System;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Nofdev.Multitenancy.Identity.EntityFramework
{
    /// <summary>
    /// Minimal class for a <see cref="MultitenancyIdentityUserLogin{TKey, TTenant}"/> with a <see cref="string"/> user
    /// <see cref="IdentityUserLogin{TKey}.UserId"/>
    /// and <see cref="MultitenancyIdentityUserLogin{TKey, TTenant}.TenantId"/>.
    /// </summary>
    public class MultitenancyIdentityUserLogin : MultitenancyIdentityUserLogin<string, string>
    {
    }

    /// <summary>
    /// Class defining a multi-tenant user login.
    /// </summary>
    /// <typeparam name="TKey">The type of <see cref="IdentityUserLogin{TKey}.UserId"/> for a user.</typeparam>
    /// <typeparam name="TTenantKey">The type of <see cref="TenantId"/> for a user.</typeparam>
    public class MultitenancyIdentityUserLogin<TKey, TTenantKey> : IdentityUserLogin<TKey> where TKey : IEquatable<TKey>
    {
        /// <summary>
        /// Gets or sets the unique identifier of the tenant.
        /// </summary>
        public virtual TTenantKey TenantId { get; set; }
    }
}
