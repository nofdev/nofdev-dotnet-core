using System;
using Nofdev.Core.Domain;

namespace Nofdev.Multitenancy.Identity.EntityFramework
{
    public class MultitenancyOrganizationUnit : MultitenancyOrganizationUnit<int, string, string>
    {
        
    }

    public class MultitenancyOrganizationUnit<T,TUser, TTenant> : OrganizationUnit<T, TUser>,ITenant<TTenant>
    {
        #region Implementation of ITenant<TTenant>

        public TTenant TenantId { get; set; }

        #endregion
    }
}