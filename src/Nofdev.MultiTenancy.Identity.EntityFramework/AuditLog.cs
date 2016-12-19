using Nofdev.Core.Domain;

namespace Nofdev.Multitenancy.Identity.EntityFramework
{
    public class AuditLog<T,TUser> : ImmutableModel<T, TUser>
    {
        public string ClientName { get; set; }
        public string ClientVersion { get; set; }
        public string ClientIP { get; set; }
        public string UserName { get; set; }
        public string CustomData { get; set; }
    }
}