using System;
using Nofdev.Core.Domain;

namespace Nofdev.Multitenancy.Identity.EntityFramework
{
    public class MultitenancyOrganizationUnit : MultitenancyOrganizationUnit<string, string, string>
    {
        
    }

    public class MultitenancyOrganizationUnit<T,TUserKey, TTenant> : OrganizationUnit<T, TUserKey>,ITenant<TTenant>
    {
        #region Implementation of ITenant<TTenant>

        public TTenant TenantId { get; set; }

        #endregion
    }
}