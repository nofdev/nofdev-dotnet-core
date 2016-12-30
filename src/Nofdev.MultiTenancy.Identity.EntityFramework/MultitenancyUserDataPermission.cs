namespace Nofdev.Multitenancy.Identity.EntityFramework
{
    public class MultitenancyUserDataPermission : MultitenancyUserDataPermission<int, string, string>
    {
    }

    public class MultitenancyUserDataPermission<TKey, TUserKey, TTenant> :
        UserDataPermission<TKey, TUserKey>
    {
        public TTenant TenantId { get; set; }
    }
}