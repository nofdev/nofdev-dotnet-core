using System;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Nofdev.Core.Domain;

namespace Nofdev.Multitenancy.Identity.EntityFramework
{
    public class MultitenancyRoleClaim : MultitenancyRoleClaim<string,string>
    {
        
    }

    public class MultitenancyRoleClaim<TKey, TTenantKey> : IdentityRoleClaim<TKey>, ITenant<TTenantKey> where TKey : IEquatable<TKey>
    {
        public TTenantKey TenantId { get; set; }
    }
}