using System;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Nofdev.Core.Domain;


namespace Nofdev.Multitenancy.Identity.EntityFramework
{
    public class MultitenancyIdentityRole : MultitenancyIdentityRole<string, string, MultitenancyIdentityUserRole,MultitenancyIdentityRoleClaim>
    {
        
    }

    public class MultitenancyIdentityRole<TKey, TTenantKey, TUserRole, TRoleClaim> : IdentityRole<TKey,TUserRole,TRoleClaim>, ITenant<TTenantKey> where TKey : IEquatable<TKey>
    where TUserRole : MultitenancyIdentityUserRole<TKey,TTenantKey>
    where TRoleClaim : MultitenancyIdentityRoleClaim<TKey, TTenantKey>
    {
        public TTenantKey TenantId { get; set; }
    }

   
}