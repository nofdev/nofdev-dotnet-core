using System;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Nofdev.Core.Domain;

namespace Nofdev.Multitenancy.Identity.EntityFramework
{
    public class MultitenanyIdentityUserRole : MultitenanyIdentityUserRole<string, string>
    {
        
    }

    public class MultitenanyIdentityUserRole<TKey, TTenantKey> : IdentityUserRole<TKey>, ITenant<TTenantKey> where TKey : IEquatable<TKey>
    {
        public TTenantKey TenantId { get; set; }
    }
}