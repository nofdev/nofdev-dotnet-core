//-----------------------------------------------------------------------
// <copyright company="James Skimming">
//     Copyright (c) 2013 James Skimming
// </copyright>
//-----------------------------------------------------------------------

using System;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Nofdev.MultiTenant.Identity.EntityFramework
{
    /// <summary>
    /// Identity <see cref="DbContext"/> for multi tenant user accounts.
    /// </summary>
    /// <typeparam name="TUser">The type of user.</typeparam>
    /// <typeparam name="TRole">The type of role.</typeparam>
    /// <typeparam name="TKey">The type of <see cref="IUser{TKey}.Id"/> for a user.</typeparam>
    /// <typeparam name="TTenantKey">The type of <see cref="IMultiTenantUser{TTenantKey}.TenantId"/> for a user.</typeparam>
    /// <typeparam name="TUserLogin">The type of user login.</typeparam>
    /// <typeparam name="TUserRole">The type of user role.</typeparam>
    /// <typeparam name="TUserClaim">The type of user claim.</typeparam>
    /// <typeparam name="TRoleClaim"></typeparam>
    /// <typeparam name="TUserToken"></typeparam>
    public class MultiTenantIdentityDbContext<TUser, TRole, TKey, TTenantKey,  TUserClaim,TUserRole,TUserLogin,  TRoleClaim, TUserToken>
        : IdentityDbContext<TUser, TRole, TKey, TUserClaim, TUserRole, TUserLogin,TRoleClaim, TUserToken>
        where TUser : MultiTenantIdentityUser<TKey, TTenantKey, TUserLogin, TUserRole, TUserClaim>
        where TRole : IdentityRole<TKey, TUserRole,TRoleClaim>
        where TRoleClaim : IdentityRoleClaim<TKey>
        where TUserLogin : MultiTenantIdentityUserLogin<TKey, TTenantKey>, new()
        where TUserRole : IdentityUserRole<TKey>, new()
        where TUserClaim : IdentityUserClaim<TKey>, new()
          where TUserToken : IdentityUserToken<TKey>
         where TKey : IEquatable<TKey>
    {
     
        ///// <summary>
        ///// Performs custom validation.
        ///// </summary>
        ///// <param name="entityEntry"><see cref="DbEntityEntry"/> instance to be validated. </param>
        ///// <param name="items">User-defined dictionary containing additional info for custom validation. It will be
        ///// passed to ValidationContext and will be exposed as ValidationContext.Items.
        ///// This parameter is optional and can be null.
        ///// </param>
        ///// <returns>Entity validation result. Possibly null when overridden.</returns>
        //protected override DbEntityValidationResult ValidateEntity(
        //    DbEntityEntry entityEntry,
        //    IDictionary<object, object> items)
        //{
        //    if (entityEntry != null && entityEntry.State == EntityState.Added)
        //    {
        //        var user = entityEntry.Entity as TUser;
        //        if (user != null)
        //        {
        //            // TODO Perform Custom Validation.
        //            return new DbEntityValidationResult(entityEntry, Enumerable.Empty<DbValidationError>());
        //        }
        //    }

        //    return base.ValidateEntity(entityEntry, items);
        //}
        

        #region Overrides of IdentityDbContext<TUser,TRole,TKey,TUserClaim,TUserRole,TUserLogin,TRoleClaim,TUserToken>

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<TUserLogin>()
                .HasKey(e => new { e.TenantId, e.LoginProvider, e.ProviderKey, e.UserId });

            modelBuilder.Entity<TUser>()
                .HasIndex(m => m.UserName).IsUnique();

        }

        #endregion
    }
}
