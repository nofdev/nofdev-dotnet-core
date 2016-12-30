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
        : MultitenancyIdentityDbContext<TUser, MultitenancyRole, MultitenancyOrganizationUnit, string, string,
            MultitenancyUserClaim, MultitenancyUserRole,
            MultitenancyUserLogin, MultitenancyUserToken,
            MultitenancyUserPermission,
            MultitenancyUserOrganization,
             MultitenancyUserDataPermission,
              MultitenancyRoleClaim, MultitenancyRolePermission,
 MultitenancyAuditLog>
        where TUser :
            MultitenancyUser
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
    /// <typeparam name="TRoleClaim">The type of role claim.</typeparam>
    /// <typeparam name="TUserToken">The type of user token.</typeparam>
    /// <typeparam name="TRolePermission">The type of role permission.</typeparam>
    /// <typeparam name="TUserPermission">The type of user permission.</typeparam>
    /// <typeparam name="TOrg">The type of organization unit.</typeparam>
    /// <typeparam name="TUserOrg">The type of user organization relationship.</typeparam>
    /// <typeparam name="TUserDataPermission">The type of user data permission.</typeparam>
    /// <typeparam name="TLog">The type of audit log.</typeparam>
    public class MultitenancyIdentityDbContext<TUser, TRole, TOrg, TKey, TTenantKey, TUserClaim, TUserRole, TUserLogin,
        TUserToken, TUserPermission, TUserOrg,TUserDataPermission, TRoleClaim, TRolePermission, TLog>
        : IdentityDbContext<TUser, TRole, TKey, TUserClaim, TUserRole, TUserLogin, TRoleClaim, TUserToken>
        where TUser : MultitenancyUser<TKey, TTenantKey, TUserLogin, TUserRole, TUserClaim>
        where TRole : MultitenancyRole<TKey, TTenantKey, TUserRole, TRoleClaim>
        where TRoleClaim : MultitenancyRoleClaim<TKey, TTenantKey>
        where TUserLogin : MultitenancyUserLogin<TKey, TTenantKey>, new()
        where TUserRole : MultitenancyUserRole<TKey, TTenantKey>, new()
        where TUserClaim : MultitenancyUserClaim<TKey, TTenantKey>, new()
        where TUserToken : MultitenancyUserToken<TKey, TTenantKey>
        where TRolePermission : MultitenancyRolePermission<int, TKey, TKey, TTenantKey, TKey>
        where TUserPermission : MultitenancyUserPermission<int, TKey, TKey, TTenantKey>
        where TUserDataPermission : MultitenancyUserDataPermission<int, TKey, TTenantKey>
        where TOrg : MultitenancyOrganizationUnit<TKey, TKey, TTenantKey>
        where TUserOrg : MultitenancyUserOrganization<int, TKey, TKey, TTenantKey>
        where TLog : MultitenancyAuditLog<TKey, TKey, TTenantKey>
        where TKey : IEquatable<TKey>
    {
        public DbSet<TRolePermission> RolePermissions { get; set; }
        public DbSet<TUserPermission> UserPermissions { get; set; }
        public DbSet<TUserDataPermission> UserDataPermissions { get; set; } 
        public DbSet<TOrg> OrganizationUnits { get; set; }
        public DbSet<TUserOrg> UserOrganizations { get; set; }
        public DbSet<TLog> AuditLogs { get; set; }

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