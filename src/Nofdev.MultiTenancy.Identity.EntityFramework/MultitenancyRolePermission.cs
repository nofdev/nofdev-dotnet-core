namespace Nofdev.Multitenancy.Identity.EntityFramework
{
    public class MultitenancyRolePermission : MultitenancyRolePermission<string, string, string, string, string>
    {
    }

    public class MultitenancyRolePermission<TKey, TRole, TPermission, TTenant, TUser> :
        RolePermission<TKey, TRole, TPermission, TUser>
    {
        public TTenant TenantId { get; set; }
    }
}