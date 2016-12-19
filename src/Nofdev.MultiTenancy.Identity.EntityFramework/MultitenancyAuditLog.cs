using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Nofdev.Core.Domain;

namespace Nofdev.Multitenancy.Identity.EntityFramework
{
    public class MultitenancyAuditLog : MultitenancyAuditLog<long, string, string>
    {
        
    }

    public class MultitenancyAuditLog<T, TUser,TTenant> : AuditLog<T, TUser>, ITenant<TTenant>
    {
        #region Implementation of ITenant<TTenant>

        public TTenant TenantId { get; set; }

        #endregion
    }
}
