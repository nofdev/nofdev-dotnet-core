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
        : MultitenancyIdentityDbContext<TUser, MultitenancyIdentityRole, string, string,
            MultitenancyIdentityUserClaim, MultitenancyIdentityUserRole,
            MultitenancyIdentityUserLogin, MultitenancyIdentityRoleClaim, MultitenancyIdentityUserToken,
            string, MultitenancyRolePermission<string, MultitenancyIdentityRole, string, string, TUser>,
            MultitenancyUserPermission<string, TUser, string, string>,
            MultitenancyOrganizationUnit<string, TUser, string>,
            MultitenancyUserOrganization<string, TUser,MultitenancyOrganizationUnit<string, TUser, string>, string>,
             MultitenancyUserDataPermission<string, TUser, string>,
 MultitenancyAuditLog<string, TUser, string>>
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
    /// <typeparam name="TRoleClaim">The type of role claim.</typeparam>
    /// <typeparam name="TUserToken">The type of user token.</typeparam>
    /// <typeparam name="TPermission">The type of permission key.</typeparam>
    /// <typeparam name="TRolePermission">The type of role permission.</typeparam>
    /// <typeparam name="TUserPermission">The type of user permission.</typeparam>
    /// <typeparam name="TOrg">The type of organization unit.</typeparam>
    /// <typeparam name="TUserOrg">The type of user organization relationship.</typeparam>
    /// <typeparam name="TDataPermission">The type of user data permission.</typeparam>
    /// <typeparam name="TLog">The type of audit log.</typeparam>
    public class MultitenancyIdentityDbContext<TUser, TRole, TKey, TTenantKey, TUserClaim, TUserRole, TUserLogin,
        TRoleClaim, TUserToken,
        TPermission, TRolePermission, TUserPermission, TOrg, TUserOrg,TDataPermission, TLog>
        : IdentityDbContext<TUser, TRole, TKey, TUserClaim, TUserRole, TUserLogin, TRoleClaim, TUserToken>
        where TUser : MultitenancyIdentityUser<TKey, TTenantKey, TUserLogin, TUserRole, TUserClaim>
        where TRole : MultitenancyIdentityRole<TKey, TTenantKey, TUserRole, TRoleClaim>
        where TRoleClaim : MultitenancyIdentityRoleClaim<TKey, TTenantKey>
        where TUserLogin : MultitenancyIdentityUserLogin<TKey, TTenantKey>, new()
        where TUserRole : MultitenancyIdentityUserRole<TKey, TTenantKey>, new()
        where TUserClaim : MultitenancyIdentityUserClaim<TKey, TTenantKey>, new()
        where TUserToken : MultitenancyIdentityUserToken<TKey, TTenantKey>
        where TRolePermission : MultitenancyRolePermission<TKey, TRole, TPermission, TTenantKey, TUser>
        where TUserPermission : MultitenancyUserPermission<TKey, TUser, TPermission, TTenantKey>
        where TDataPermission : MultitenancyUserDataPermission<TKey,TUser,TTenantKey>
        where TOrg : MultitenancyOrganizationUnit<TKey, TUser, TTenantKey>
        where TUserOrg : MultitenancyUserOrganization<TKey,TUser,TOrg,TTenantKey>
        where TLog : MultitenancyAuditLog<TKey, TUser, TTenantKey>
        where TKey : IEquatable<TKey>
    {
        public DbSet<TRolePermission> RolePermissions { get; set; }
        public DbSet<TUserPermission> UserPermissions { get; set; }
        public DbSet<TDataPermission> UserDataPermissions { get; set; } 
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