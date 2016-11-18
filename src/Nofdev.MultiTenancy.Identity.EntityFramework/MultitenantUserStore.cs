using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

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
                <TUser, IdentityRole, TContext, string, int, IdentityUserClaim<string>, IdentityUserRole<string>,
                    MultitenancyIdentityUserLogin, IdentityUserToken<string>>
        where TUser : MultitenancyIdentityUser
        where TContext : DbContext
    {
        public MultitenancyUserStore(TContext context, IdentityErrorDescriber describer = null)
            : base(context, describer)
        {
        }
    }
}