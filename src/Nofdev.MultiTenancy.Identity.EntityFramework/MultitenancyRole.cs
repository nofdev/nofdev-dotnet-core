using System;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Nofdev.Core.Domain;


namespace Nofdev.Multitenancy.Identity.EntityFramework
{
    public class MultitenancyRole : MultitenancyRole<string, string, MultitenancyUserRole,MultitenancyRoleClaim>
    {
        
    }

    public class MultitenancyRole<TKey, TTenantKey, TUserRole, TRoleClaim> : IdentityRole<TKey,TUserRole,TRoleClaim>, ITenant<TTenantKey> where TKey : IEquatable<TKey>
    where TUserRole : MultitenancyUserRole<TKey,TTenantKey>
    where TRoleClaim : MultitenancyRoleClaim<TKey, TTenantKey>
    {
        public TTenantKey TenantId { get; set; }
    }

   
}