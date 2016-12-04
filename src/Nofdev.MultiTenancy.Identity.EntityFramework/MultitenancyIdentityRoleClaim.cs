using System;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Nofdev.Core.Domain;

namespace Nofdev.Multitenancy.Identity.EntityFramework
{
    public class MultitenancyIdentityRoleClaim : MultitenancyIdentityRoleClaim<string,string>
    {
        
    }

    public class MultitenancyIdentityRoleClaim<TKey, TTenantKey> : IdentityRoleClaim<TKey>, ITenant<TTenantKey> where TKey : IEquatable<TKey>
    {
        public TTenantKey TenantId { get; set; }
    }
}