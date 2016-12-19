using Nofdev.Core.Domain;

namespace Nofdev.Multitenancy.Identity.EntityFramework
{
    public class UserOrganization<T, TUser, TOrg> : ImmutableModel<T,TUser>
    {
        public TUser UserId { get; set; }
        public TOrg OrganizationUnitId { get; set; }
    }
}