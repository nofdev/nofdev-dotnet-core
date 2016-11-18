//-----------------------------------------------------------------------
// <copyright company="James Skimming">
//     Copyright (c) 2013 James Skimming
// </copyright>
//-----------------------------------------------------------------------

using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Nofdev.Core.Domain;

namespace Nofdev.Multitenancy.Identity.EntityFramework
{
    /// <summary>
    /// The store for a multi tenant user.
    /// </summary>
    /// <typeparam name="TUser">The type of user.</typeparam>
    /// <typeparam name="TRole">The type of role.</typeparam>
    /// <typeparam name="TKey">The type of <see cref="IUser{TKey}.Id"/> for a user.</typeparam>
    /// <typeparam name="TTenantKey">The type of <see cref="ITenant.TenantId"/> for a user.</typeparam>
    /// <typeparam name="TUserLogin">The type of user login.</typeparam>
    /// <typeparam name="TUserRole">The type of user role.</typeparam>
    /// <typeparam name="TUserClaim">The type of user claim.</typeparam>
    /// <typeparam name="TContext"></typeparam>
    /// <typeparam name="TUserToken"></typeparam>
    public class MultitenancyUserStore<TUser, TRole, TContext, TKey, TTenantKey, TUserClaim, TUserRole, TUserLogin,TUserToken>
        : UserStore<TUser, TRole, TContext, TKey, TUserClaim, TUserRole, TUserLogin,TUserToken>
        where TUser : MultitenancyIdentityUser<TKey, TTenantKey, TUserLogin, TUserRole, TUserClaim>
        where TRole : IdentityRole<TKey, TUserRole, IdentityRoleClaim<TKey>>
        where TKey : IEquatable<TKey>
        where TTenantKey : IEquatable<TTenantKey>
        where TUserLogin : MultitenancyIdentityUserLogin<TKey, TTenantKey>, new()
        where TUserRole : IdentityUserRole<TKey>, new()
        where TUserClaim : IdentityUserClaim<TKey>, new()
        where TContext : DbContext 
        where TUserToken : IdentityUserToken<TKey>
    {
        /// <summary>
        /// Flag indicating whether this object has been disposed.
        /// </summary>
        private bool _disposed;

        /// <summary>
        /// Backing field for the <see cref="Logins"/> property.
        /// </summary>
        private DbSet<TUserLogin> _logins;


        /// <summary>
        /// Gets or sets the <see cref="ITenant{TTenantKey}.TenantId"/> to be used in queries.
        /// </summary>
        public virtual TTenantKey TenantId { get; set; }

        /// <summary>
        /// Gets the set of users.
        /// </summary>
        private DbSet<TUserLogin> Logins
        {
            get { return _logins ?? (_logins = Context.Set<TUserLogin>()); }
        }

        ///// <summary>
        ///// Creates a new user.
        ///// </summary>
        ///// <param name="user">The user to create.</param>
        ///// <returns>An <see cref="Task"/> from which the operation can be awaited.</returns>
        //public override Task CreateAsync(TUser user)
        //{
        //    if (user == null)
        //        throw new ArgumentNullException("user");

        //    ThrowIfInvalid();

        //    // Need to set the tenant Id.
        //    user.TenantId = TenantId;

        //    return base.CreateAsync(user);
        //}

        ///// <summary>
        ///// Finds a <typeparamref name="TUser"/> by their <see cref="IUser{TKey}.UserName"/>.
        ///// </summary>
        ///// <param name="userName">The <see cref="IUser{TKey}.UserName"/> of a <typeparamref name="TUser"/>.</param>
        ///// <returns>The <typeparamref name="TUser"/> if found; otherwise <c>null</c>.</returns>
        //public override Task<TUser> FindByNameAsync(string userName)
        //{
        //    ThrowIfInvalid();
        //    return GetUserAggregateAsync(u => u.UserName == userName && u.TenantId.Equals(TenantId));
        //}

        ///// <summary>
        ///// Adds the external login for the <paramref name="user"/>.
        ///// </summary>
        ///// <param name="user">The user.</param>
        ///// <param name="login">The login info.</param>
        ///// <returns>An <see cref="Task"/> from which the operation can be awaited.</returns>
        //public override Task AddLoginAsync(TUser user, UserLoginInfo login)
        //{
        //    if (user == null)
        //        throw new ArgumentNullException("user");
        //    if (login == null)
        //        throw new ArgumentNullException("login");

        //    ThrowIfInvalid();

        //    var userLogin = new TUserLogin
        //        {
        //            TenantId = TenantId,
        //            UserId = user.Id,
        //            ProviderKey = login.ProviderKey,
        //            LoginProvider = login.LoginProvider,
        //        };

        //    user.Logins.Add(userLogin);
        //    return Task.FromResult(0);
        //}

        ///// <summary>
        ///// Fins a user bases on their external login.
        ///// </summary>
        ///// <param name="login">The login info.</param>
        ///// <returns>The user if found, otherwise <c>null</c>.</returns>
        //public override async Task<TUser> FindAsync(UserLoginInfo login)
        //{
        //    if (login == null)
        //        throw new ArgumentNullException("login");

        //    ThrowIfInvalid();

        //    TKey userId = await
        //        (from l in Logins
        //         where l.LoginProvider == login.LoginProvider
        //               && l.ProviderKey == login.ProviderKey
        //               && l.TenantId.Equals(TenantId)
        //         select l.UserId)
        //            .SingleOrDefaultAsync()
        //            .ConfigureAwait(false);

        //    if (EqualityComparer<TKey>.Default.Equals(userId, default(TKey)))
        //        return null;

        //    return await GetUserAggregateAsync(u => u.Id.Equals(userId));
        //}

        ///// <summary>
        ///// Find a user by email
        ///// </summary>
        ///// <param name="email">The Email address of a <typeparamref name="TUser"/>.</param>
        ///// <returns>The <typeparamref name="TUser"/> if found; otherwise <c>null</c>.</returns>
        //public override Task<TUser> FindByEmailAsync(string email)
        //{
        //    ThrowIfInvalid();
        //    return GetUserAggregateAsync(u => u.Email.ToUpper() == email.ToUpper() && u.TenantId.Equals(TenantId));
        //}

        ///// <summary>
        ///// Cleanly disposes of this object.
        ///// </summary>
        ///// <param name="disposing"><c>true</c> if the object is being disposed; otherwise <c>false</c>.</param>
        //protected override void Dispose(bool disposing)
        //{
        //    if (!disposing || _disposed)
        //        return;

        //    _disposed = true;
        //    _logins = null;
        //}

        /// <summary>
        /// Throws exceptions if the state of the object is invalid or has been disposed.
        /// </summary>
        private void ThrowIfInvalid()
        {
            if (_disposed)
                throw new ObjectDisposedException(GetType().Name);

            if (EqualityComparer<TTenantKey>.Default.Equals(TenantId, default(TTenantKey)))
                throw new InvalidOperationException("The TenantId has not been set.");
        }

        #region Overrides of UserStore<TUser,TRole,TContext,TKey,TUserClaim,TUserRole,TUserLogin,TUserToken>

        protected override TUserRole CreateUserRole(TUser user, TRole role)
        {
            throw new NotImplementedException();
        }

        protected override TUserClaim CreateUserClaim(TUser user, Claim claim)
        {
            throw new NotImplementedException();
        }

        protected override TUserLogin CreateUserLogin(TUser user, UserLoginInfo login)
        {
            throw new NotImplementedException();
        }

        protected override TUserToken CreateUserToken(TUser user, string loginProvider, string name, string value)
        {
            throw new NotImplementedException();
        }


        #endregion

        public MultitenancyUserStore(TContext context, IdentityErrorDescriber describer = null) : base(context, describer)
        {
        }
    }
}
