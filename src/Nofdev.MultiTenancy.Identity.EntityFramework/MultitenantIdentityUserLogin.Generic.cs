//-----------------------------------------------------------------------
// <copyright company="James Skimming">
//     Copyright (c) 2013 James Skimming
// </copyright>
//-----------------------------------------------------------------------

using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;

namespace Nofdev.Multitenancy.Identity.EntityFramework
{
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
