using System;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Nofdev.Core.Domain;

namespace Nofdev.Multitenancy.Identity.EntityFramework
{
    public class MultitenancyIdentityUserRole : MultitenancyIdentityUserRole<string, string>
    {
        
    }

    public class MultitenancyIdentityUserRole<TKey, TTenantKey> : IdentityUserRole<TKey>, ITenant<TTenantKey> where TKey : IEquatable<TKey>
    {
        public TTenantKey TenantId { get; set; }
    }

   
}