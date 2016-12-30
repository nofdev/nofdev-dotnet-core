using System;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Nofdev.Core.Domain;

namespace Nofdev.Multitenancy.Identity.EntityFramework
{
    public class MultitenancyUserToken : MultitenancyUserToken<string, string>
    {
    }

    public class MultitenancyUserToken<TUserKey, TTenantKey> : IdentityUserToken<TUserKey>, ITenant<TTenantKey>
        where TUserKey : IEquatable<TUserKey>
    {
        public TTenantKey TenantId { get; set; }
    }
}