using System;
using Nofdev.Core.Domain;
using Nofdev.Core.SOA;

namespace Nofdev.Core.Util
{
    public static class TenantChecker
    {
        public static void AssignTenantId(this object entity)
        {
            if (string.IsNullOrWhiteSpace(ServiceContext.Current.User?.TenantId))
                return;
            var o = entity as ITenant;
            if (o != null)
            {
                o.TenantId = ServiceContext.Current.User.TenantId;
            }
        }

        public static void CheckTenant(this  object entity)
        {
            if (string.IsNullOrWhiteSpace(ServiceContext.Current.User?.TenantId))
                return;
            var o = entity as ITenant;
            if (o != null)
            {
                if (o.TenantId != ServiceContext.Current.User.TenantId)
                {
                    throw new UnauthorizedAccessException("You are not the owner of this resource.");
                }
            }
        }

    }
}
