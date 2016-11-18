
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Nofdev.MultiTenant.Identity.EntityFramework
{
    /// <summary>
    ///     The store for a multi tenant user.
    /// </summary>
    /// <typeparam name="TUser">The type of user.</typeparam>
    /// <typeparam name="TContext"></typeparam>
    public class MultiTenantUserStore<TUser, TContext>
        :
            MultiTenantUserStore
                <TUser, IdentityRole, TContext, string, int, IdentityUserClaim<string>, IdentityUserRole<string>,
                    MultiTenantIdentityUserLogin, IdentityUserToken<string>>
        where TUser : MultiTenantIdentityUser
        where TContext : DbContext
    {
        public MultiTenantUserStore(TContext context, IdentityErrorDescriber describer = null)
            : base(context, describer)
        {
        }
    }
}