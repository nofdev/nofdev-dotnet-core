using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using Nofdev.Core.Domain;

namespace Nofdev.Multitenancy.Identity.EntityFramework
{
    /// <summary>
    /// Minimal class for a <see cref="MultitenancyIdentityUser{TKey, TTenantKey, TLogin, TRole, TClaim}"/> with a
    /// <see cref="string"/> user <see cref="IdentityUser{TKey}.Id"/> and
    /// <see cref="MultitenancyIdentityUserLogin{TKey, TTenant}.TenantId"/>.
    /// </summary>
    public class MultitenancyIdentityUser :
        MultitenancyIdentityUser<string, string, MultitenancyIdentityUserLogin, MultitenancyIdentityUserRole, MultitenancyIdentityUserClaim>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MultitenancyIdentityUser"/> class.
        /// </summary>
        public MultitenancyIdentityUser()
        {
            Id = Guid.NewGuid().ToString();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MultitenancyIdentityUser"/> class.
        /// </summary>
        /// <param name="userName">The <see cref="IdentityUser{TKey, TLogin, TRole, TClaim}.UserName"/> of the user.</param>
        public MultitenancyIdentityUser(string userName)
            : this()
        {
            if (string.IsNullOrWhiteSpace(userName))
                throw new ArgumentNullException("userName");

            UserName = userName;
        }
    }

    /// <summary>
    /// Class defining a multi-tenant user.
    /// </summary>
    /// <typeparam name="TKey">The type of <see cref="IdentityUser.Id"/> for a user.</typeparam>
    /// <typeparam name="TTenantKey">The type of <see cref="ITenant{TTenantKey}.TenantId"/> for a user.</typeparam>
    /// <typeparam name="TLogin">The type of user login.</typeparam>
    /// <typeparam name="TRole">The type of user role.</typeparam>
    /// <typeparam name="TClaim">The type of user claim.</typeparam>
    public class MultitenancyIdentityUser<TKey, TTenantKey, TLogin, TRole, TClaim>
        : IdentityUser<TKey, TClaim, TRole, TLogin>, ITenant<TTenantKey>
        where TLogin : MultitenancyIdentityUserLogin<TKey, TTenantKey>
        where TRole : IdentityUserRole<TKey>
        where TClaim : IdentityUserClaim<TKey>
        where TKey : IEquatable<TKey>
    {
        /// <summary>
        /// Gets or sets the unique identifier of the tenant.
        /// </summary>
        public TTenantKey TenantId { get; set; }
    }
}
