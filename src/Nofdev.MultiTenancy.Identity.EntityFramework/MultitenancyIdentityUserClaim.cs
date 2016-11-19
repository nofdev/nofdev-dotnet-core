using System;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Nofdev.Core.Domain;

namespace Nofdev.Multitenancy.Identity.EntityFramework
{
    public class MultitenancyIdentityUserClaim : MultitenancyIdentityUserClaim<string, string>
    {
    }

    public class MultitenancyIdentityUserClaim<TUserKey, TTenantKey> : IdentityUserClaim<TUserKey>, ITenant<TTenantKey>
        where TUserKey : IEquatable<TUserKey>
    {
        public TTenantKey TenantId { get; set; }
    }
}