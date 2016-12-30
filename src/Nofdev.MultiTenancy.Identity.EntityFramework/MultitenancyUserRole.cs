using System;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Nofdev.Core.Domain;

namespace Nofdev.Multitenancy.Identity.EntityFramework
{
    public class MultitenancyUserRole : MultitenancyUserRole<string, string>
    {
        
    }

    public class MultitenancyUserRole<TKey, TTenantKey> : IdentityUserRole<TKey>, ITenant<TTenantKey> where TKey : IEquatable<TKey>
    {
        public TTenantKey TenantId { get; set; }
    }

   
}