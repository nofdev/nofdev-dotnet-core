using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;

namespace Nofdev.Multitenancy.Identity.EntityFramework
{
    /// <summary>
    /// Minimal class for a <see cref="MultitenancyIdentityUser{TKey, TTenantKey, TLogin, TRole, TClaim}"/> with a
    /// <see cref="string"/> user <see cref="IUser{TKey}.Id"/> and
    /// <see cref="MultitenancyIdentityUserLogin{TKey, TTenant}.TenantId"/>.
    /// </summary>
    public class MultitenancyIdentityUser :
        MultitenancyIdentityUser<string, int, MultitenancyIdentityUserLogin, IdentityUserRole<string>, IdentityUserClaim<string>>
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
}
