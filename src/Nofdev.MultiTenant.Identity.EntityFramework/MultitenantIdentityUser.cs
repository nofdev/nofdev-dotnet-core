
using System;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Nofdev.MultiTenant.Identity.EntityFramework
{
    /// <summary>
    /// Minimal class for a <see cref="MultiTenantIdentityUser{TKey, TTenantKey, TLogin, TRole, TClaim}"/> with a
    /// <see cref="string"/> user <see cref="IUser{TKey}.Id"/> and
    /// <see cref="MultiTenantIdentityUserLogin{TKey, TTenant}.TenantId"/>.
    /// </summary>
    public class MultiTenantIdentityUser :
        MultiTenantIdentityUser<string, string, MultiTenantIdentityUserLogin, IdentityUserRole<string>, IdentityUserClaim<string>>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MultiTenantIdentityUser"/> class.
        /// </summary>
        public MultiTenantIdentityUser()
        {
            Id = Guid.NewGuid().ToString();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MultiTenantIdentityUser"/> class.
        /// </summary>
        /// <param name="userName">The <see cref="IdentityUser{TKey, TLogin, TRole, TClaim}.UserName"/> of the user.</param>
        public MultiTenantIdentityUser(string userName)
            : this()
        {
            if (string.IsNullOrWhiteSpace(userName))
                throw new ArgumentNullException("userName");

            UserName = userName;
        }
    }
}
