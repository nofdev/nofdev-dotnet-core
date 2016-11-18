//-----------------------------------------------------------------------
// <copyright company="James Skimming">
//     Copyright (c) 2013 James Skimming
// </copyright>
//-----------------------------------------------------------------------

namespace Nofdev.MultiTenant.Identity.EntityFramework
{
    /// <summary>
    /// Interface defining a multi-tenant user.
    /// </summary>
    /// <typeparam name="TTenantKey">The type of <see cref="TenantId"/> for a user.</typeparam>
    public interface IMultiTenantUser<TTenantKey> 
    {
        /// <summary>
        /// Gets or sets the unique identifier of the tenant.
        /// </summary>
        TTenantKey TenantId { get; set; } 
    }
}
