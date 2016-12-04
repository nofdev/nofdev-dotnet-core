using System;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Nofdev.Core.Domain;

namespace Nofdev.Multitenancy.Identity.EntityFramework
{
    /// <summary>
    ///     Identity <see cref="DbContext" /> for multi tenant user accounts.
    /// </summary>
    /// <typeparam name="TUser">The type of user.</typeparam>
    public class MultitenancyIdentityDbContext<TUser>
        : MultitenancyIdentityDbContext<TUser,MultitenancyIdentityRole,string, string,
                    MultitenancyIdentityUserClaim, MultitenancyIdentityUserRole,
                    MultitenancyIdentityUserLogin, MultitenancyIdentityRoleClaim, MultitenancyIdentityUserToken>
        where TUser :
            MultitenancyIdentityUser
                <string, string, MultitenancyIdentityUserLogin, MultitenancyIdentityUserRole,
                    MultitenancyIdentityUserClaim>
    {
        ///// <summary>
        ///// Applies custom model definitions for multi-tenancy.
        ///// </summary>
        ///// <param name="modelBuilder">The builder that defines the model for the context being created. </param>
        //protected override void OnModelCreating(DbModelBuilder modelBuilder)
        //{
        //    base.OnModelCreating(modelBuilder);

        //    modelBuilder.Entity<TUser>()
        //        .Property(e => e.TenantId)
        //        .HasMaxLength(128)
        //        .IsRequired()
        //        .HasColumnAnnotation(
        //            "Index",
        //            new IndexAnnotation(new IndexAttribute("UserNameIndex", order: 0)
        //                {
        //                    IsUnique = true
        //                }));
        //}
    }

    /// <summary>
    ///     Identity <see cref="DbContext" /> for multi tenant user accounts.
    /// </summary>
    /// <typeparam name="TUser">The type of user.</typeparam>
    /// <typeparam name="TRole">The type of role.</typeparam>
    /// <typeparam name="TKey">The type of <see cref="IdentityUser{TKey}.Id" /> for a user.</typeparam>
    /// <typeparam name="TTenantKey">The type of <see cref="ITenant.TenantId" /> for a user.</typeparam>
    /// <typeparam name="TUserLogin">The type of user login.</typeparam>
    /// <typeparam name="TUserRole">The type of user role.</typeparam>
    /// <typeparam name="TUserClaim">The type of user claim.</typeparam>
    /// <typeparam name="TRoleClaim"></typeparam>
    /// <typeparam name="TUserToken"></typeparam>
    public class MultitenancyIdentityDbContext<TUser, TRole, TKey, TTenantKey, TUserClaim, TUserRole, TUserLogin,TRoleClaim, TUserToken>
        : IdentityDbContext<TUser, TRole, TKey, TUserClaim, TUserRole, TUserLogin, TRoleClaim, TUserToken>
        where TUser : MultitenancyIdentityUser<TKey, TTenantKey, TUserLogin, TUserRole, TUserClaim>
        where TRole : MultitenancyIdentityRole<TKey, TTenantKey, TUserRole, TRoleClaim>
        where TRoleClaim : MultitenancyIdentityRoleClaim<TKey,TTenantKey>
        where TUserLogin : MultitenancyIdentityUserLogin<TKey, TTenantKey>, new()
        where TUserRole : MultitenancyIdentityUserRole<TKey, TTenantKey>,  new()
        where TUserClaim : MultitenancyIdentityUserClaim<TKey, TTenantKey>, new()
        where TUserToken : MultitenancyIdentityUserToken<TKey, TTenantKey>
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
                .HasKey(e => new {e.TenantId, e.LoginProvider, e.ProviderKey, e.UserId});

            modelBuilder.Entity<TUser>()
                .HasIndex(m => m.UserName).IsUnique();
        }

        #endregion
    }
}