using System;
using Nofdev.Core.Domain;

namespace Nofdev.Multitenancy.Identity.EntityFramework
{
    public class MultitenancyUserOrganization : MultitenancyUserOrganization<int, string, string, string>
    {
        
    }

    public class MultitenancyUserOrganization<T, TUserKey, TOrgKey,TTenant> : UserOrganization<T, TUserKey,TOrgKey>, ITenant<TTenant>
    {
        #region Implementation of ITenant<TTenant>

        public TTenant TenantId { get; set; }

        #endregion
    }
}