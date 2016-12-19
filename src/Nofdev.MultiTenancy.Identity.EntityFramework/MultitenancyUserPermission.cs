using System;
using Nofdev.Core.Domain;

namespace Nofdev.Multitenancy.Identity.EntityFramework
{
    public class MultitenancyUserPermission : MultitenancyUserPermission<string, string, string, string>
    {
    }

    public class MultitenancyUserPermission<TKey, TUser, TPermission, TTenant> :
        UserPermission<TKey, TUser, TPermission>
    {
        public TTenant TenantId { get; set; }
    }

}