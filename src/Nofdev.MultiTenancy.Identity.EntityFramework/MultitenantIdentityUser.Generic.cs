﻿//-----------------------------------------------------------------------
// <copyright company="James Skimming">
//     Copyright (c) 2013 James Skimming
// </copyright>
//-----------------------------------------------------------------------

using System;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Nofdev.Core.Domain;

namespace Nofdev.Multitenancy.Identity.EntityFramework
{
    /// <summary>
    /// Class defining a multi-tenant user.
    /// </summary>
    /// <typeparam name="TKey">The type of <see cref="IdentityUser.Id"/> for a user.</typeparam>
    /// <typeparam name="TTenantKey">The type of <see cref="ITenant.TenantId"/> for a user.</typeparam>
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