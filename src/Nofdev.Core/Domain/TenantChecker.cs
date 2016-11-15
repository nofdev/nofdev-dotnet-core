using System;

namespace Nofdev.Core.Domain
{
    public static class TenantChecker
    {
        public static void AssignTenantId(this ITenantContext tenantContext, object entity)
        {
            if (tenantContext == null || tenantContext.TenantId <= 0)
                return;
            var o = entity as ITenant;
            if (o != null)
            {
                o.TenantId = tenantContext.TenantId;
            }
        }

        public static void CheckTenant(this ITenantContext tenantContext, object entity)
        {
            if (tenantContext == null || tenantContext.TenantId <= 0)
                return;
            var o = entity as ITenant;
            if (o != null)
            {
                if (o.TenantId != tenantContext.TenantId)
                {
                    throw new UnauthorizedAccessException("You are not the owner of this resource.");
                }
            }
        }

    }
}
