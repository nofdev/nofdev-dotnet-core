using System;

namespace Nofdev.Multitenancy.Identity.EntityFramework
{
    public class MultitenancyUserPermission : MultitenancyUserPermission<int, string, string, string>
    {
    }

    public class MultitenancyUserPermission<TKey, TUserKey, TPermission, TTenant> :
        UserPermission<TKey, TUserKey, TPermission>
    {
        public TTenant TenantId { get; set; }
    }
}