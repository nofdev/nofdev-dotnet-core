
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Nofdev.MultiTenant.Identity.EntityFramework
{
    /// <summary>
    ///     Identity <see cref="DbContext" /> for multi tenant user accounts.
    /// </summary>
    /// <typeparam name="TUser">The type of user.</typeparam>
    public class MultiTenantIdentityDbContext<TUser>
        :
            MultiTenantIdentityDbContext
                <TUser, IdentityRole, string, int, IdentityUserClaim<string>, IdentityUserRole<string>,
                    MultiTenantIdentityUserLogin, IdentityRoleClaim<string>, IdentityUserToken<string>>
        where TUser : MultiTenantIdentityUser
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
}