namespace Nofdev.Multitenancy.Identity.EntityFramework
{
    public class MultitenancyUserDataPermission : MultitenancyUserDataPermission<string, string, string>
    {
    }

    public class MultitenancyUserDataPermission<TKey, TUser, TTenant> :
        UserDataPermission<TKey, TUser>
    {
        public TTenant TenantId { get; set; }
    }
}