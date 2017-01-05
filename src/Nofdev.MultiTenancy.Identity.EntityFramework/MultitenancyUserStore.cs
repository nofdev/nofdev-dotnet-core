using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Nofdev.Core.Domain;

namespace Nofdev.Multitenancy.Identity.EntityFramework
{
    /// <summary>
    ///     The store for a multi tenant user.
    /// </summary>
    /// <typeparam name="TUser">The type of user.</typeparam>
    /// <typeparam name="TContext"></typeparam>
    public class MultitenancyUserStore<TUser, TContext>
        :
            MultitenancyUserStore
                <TUser, MultitenancyRole, TContext, string, string, MultitenancyUserClaim, MultitenancyUserRole,
                    MultitenancyUserLogin, MultitenancyUserToken,MultitenancyRoleClaim>
        where TUser :   MultitenancyUser
        where TContext : DbContext
    {
        public MultitenancyUserStore(TContext context, IdentityErrorDescriber describer = null)
            : base(context, describer)
        {
        }
    }

    /// <summary>
    /// The store for a multi tenant user.
    /// </summary>
    /// <typeparam name="TUser">The type of user.</typeparam>
    /// <typeparam name="TRole">The type of role.</typeparam>
    /// <typeparam name="TKey">The type of <see cref="IdentityUser{TKey}.Id"/> for a user.</typeparam>
    /// <typeparam name="TTenantKey">The type of <see cref="ITenant.TenantId"/> for a user.</typeparam>
    /// <typeparam name="TUserLogin">The type of user login.</typeparam>
    /// <typeparam name="TUserRole">The type of user role.</typeparam>
    /// <typeparam name="TUserClaim">The type of user claim.</typeparam>
    /// <typeparam name="TContext"></typeparam>
    /// <typeparam name="TUserToken"></typeparam>
    /// <typeparam name="TRoleClaim"></typeparam>
    public class MultitenancyUserStore<TUser, TRole, TContext, TKey, TTenantKey, TUserClaim, TUserRole, TUserLogin, TUserToken,TRoleClaim>
        : UserStore<TUser, TRole, TContext, TKey, TUserClaim, TUserRole, TUserLogin, TUserToken, TRoleClaim>,ITenant<TTenantKey>
           where TKey : IEquatable<TKey>
        where TTenantKey : IEquatable<TTenantKey>
        where TContext : DbContext
        where TUser : MultitenancyUser<TKey, TTenantKey, TUserLogin, TUserRole, TUserClaim>
        where TUserLogin : MultitenancyUserLogin<TKey, TTenantKey>,new()
        where TUserRole : MultitenancyUserRole<TKey, TTenantKey>, new() 
        where TUserClaim : MultitenancyUserClaim<TKey, TTenantKey>, new() 
        where TUserToken : MultitenancyUserToken<TKey, TTenantKey>, new() 
        where TRole : IdentityRole<TKey, TUserRole, TRoleClaim> 
        where TRoleClaim : MultitenancyRoleClaim<TKey,TTenantKey>
    {

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
        protected DbSet<TUserLogin> Logins => _logins ?? (_logins = Context.Set<TUserLogin>());

        public override Task<IdentityResult> CreateAsync(TUser user, CancellationToken cancellationToken = new CancellationToken())
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            ThrowIfInvalid();

            user.TenantId = TenantId;
            return base.CreateAsync(user, cancellationToken);
        }

        /// <summary>
        /// Finds a <typeparamref name="TUser"/> by their  <see cref="MultitenancyUser.NormalizedUserName"/>.
        /// </summary>
        /// <param name="normalizedUserName">The <see cref="MultitenancyUser.NormalizedUserName"/> of a <see cref="MultitenancyUser" />.</param>
        /// <param name="cancellationToken"></param>
        /// <returns>The <typeparamref name="TUser"/> if found; otherwise <c>null</c>.</returns>
        public override Task<TUser> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken = new CancellationToken())
        {
            ThrowIfInvalid();
            return Users.Where(u => u.NormalizedUserName == normalizedUserName && u.TenantId.Equals(TenantId)).SingleOrDefaultAsync(cancellationToken);
        }


        public override Task AddLoginAsync(TUser user, UserLoginInfo login, CancellationToken cancellationToken = new CancellationToken())
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));
            if (login == null)
                throw new ArgumentNullException(nameof(login));

            ThrowIfInvalid();

            var userLogin = new TUserLogin
            {
                TenantId = TenantId,
                UserId = user.Id,
                ProviderKey = login.ProviderKey,
                LoginProvider = login.LoginProvider,
            };

           user.Logins.Add(userLogin);

            return Task.FromResult(0);
        }

        /// <summary>
        /// Fins a user bases on their external login.
        /// </summary>
        /// <param name="loginProvider"></param>
        /// <param name="providerKey"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>The user if found, otherwise <c>null</c>.</returns>
        public override Task<TUser> FindByLoginAsync(string loginProvider, string providerKey,
            CancellationToken cancellationToken = new CancellationToken())
        {
            ThrowIfInvalid();

            TKey userId =  
                (from l in Logins
                 where l.LoginProvider == loginProvider
                       && l.ProviderKey == providerKey
                       && l.TenantId.Equals(TenantId)
                 select l.UserId)
                    .SingleOrDefaultAsync(cancellationToken).Result; 

            if (EqualityComparer<TKey>.Default.Equals(userId, default(TKey)))
                return null;

            return Users.Where(u => u.Id.Equals(userId)).SingleOrDefaultAsync(cancellationToken);
        }

        /// <summary>
        /// Find a user by email
        /// </summary>
        /// <param name="normalizedEmail">The Email address of a <typeparamref name="TUser"/>.</param>
        /// <param name="cancellationToken"></param>
        /// <returns>The <typeparamref name="TUser"/> if found; otherwise <c>null</c>.</returns>
        public override Task<TUser> FindByEmailAsync(string normalizedEmail, CancellationToken cancellationToken = new CancellationToken())
        {
            ThrowIfInvalid();
            return
                Users.Where(u => u.NormalizedEmail == normalizedEmail && u.TenantId.Equals(TenantId))
                    .SingleOrDefaultAsync(cancellationToken);
        }


        protected override TUserRole CreateUserRole(TUser user, TRole role)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));
            if (role == null)
                throw new ArgumentNullException(nameof(role));

            ThrowIfInvalid();

            var userRole = new TUserRole
            {
                UserId = user.Id,
                RoleId = role.Id,
                TenantId = TenantId
            };

            user.Roles.Add(userRole);

            return userRole;
        }

        protected override TUserClaim CreateUserClaim(TUser user, Claim claim)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));
            if (claim == null)
                throw new ArgumentNullException(nameof(claim));

            ThrowIfInvalid();

            var userClaim = new TUserClaim
            {
                TenantId = TenantId,
                UserId = user.Id,
                ClaimType = claim.Type,
                ClaimValue = claim.Value
            };

            user.Claims.Add(userClaim);

            return userClaim;
        }

        protected override TUserLogin CreateUserLogin(TUser user, UserLoginInfo login)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));
            if (login == null)
                throw new ArgumentNullException(nameof(login));

            ThrowIfInvalid();

            var userLogin = new TUserLogin
            {
                TenantId = TenantId,
                UserId = user.Id,
                ProviderKey = login.ProviderKey,
                LoginProvider = login.LoginProvider,
            };

            user.Logins.Add(userLogin);

            return userLogin;
        }

        protected override  TUserToken CreateUserToken(TUser user, string loginProvider, string name, string value)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            ThrowIfInvalid();

            var userToken = new TUserToken
            {
                TenantId = TenantId,
                UserId = user.Id,
                LoginProvider = loginProvider,
                Name = name,
                Value = value
            };

           SetTokenAsync(user, loginProvider, name, value, new CancellationToken()).Wait();

            return userToken;
        }



        /// <summary>
        /// Throws exceptions if the state of the object is invalid or has been disposed.
        /// </summary>
        private void ThrowIfInvalid()
        {
            if (EqualityComparer<TTenantKey>.Default.Equals(TenantId, default(TTenantKey)))
                throw new InvalidOperationException("The TenantId has not been set.");
        }



        public MultitenancyUserStore(TContext context, IdentityErrorDescriber describer = null) : base(context, describer)
        {
        }
    }
}