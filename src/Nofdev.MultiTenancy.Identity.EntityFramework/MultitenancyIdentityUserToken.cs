using System;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Nofdev.Core.Domain;

namespace Nofdev.Multitenancy.Identity.EntityFramework
{
    public class MultitenancyIdentityUserToken : MultitenancyIdentityUserToken<string, string>
    {
    }

    public class MultitenancyIdentityUserToken<TKey, TTenantKey> : IdentityUserToken<TKey>, ITenant<TTenantKey>
        where TKey : IEquatable<TKey>
    {
        public TTenantKey TenantId { get; set; }
    }
}