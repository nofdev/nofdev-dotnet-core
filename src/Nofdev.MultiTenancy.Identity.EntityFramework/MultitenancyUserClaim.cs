using System;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Nofdev.Core.Domain;

namespace Nofdev.Multitenancy.Identity.EntityFramework
{
    public class MultitenancyUserClaim : MultitenancyUserClaim<string, string>
    {
    }

    public class MultitenancyUserClaim<TUserKey, TTenantKey> : IdentityUserClaim<TUserKey>, ITenant<TTenantKey>
        where TUserKey : IEquatable<TUserKey>
    {
        public TTenantKey TenantId { get; set; }
    }
}