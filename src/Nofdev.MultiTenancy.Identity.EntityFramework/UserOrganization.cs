using Nofdev.Core.Domain;

namespace Nofdev.Multitenancy.Identity.EntityFramework
{
    public class UserOrganization<T, TUserKey, TOrgKey> : ImmutableModel<T,TUserKey>
    {
        public TUserKey UserId { get; set; }
        public TOrgKey OrganizationUnitId { get; set; }
    }
}