using Microsoft.EntityFrameworkCore;
using Nofdev.Gateway.Models;
using Nofdev.Multitenancy.Identity.EntityFramework;

namespace Nofdev.Gateway.Data
{
    public class ApplicationDbContext : MultitenancyIdentityDbContext<ApplicationUser>
    {
      
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }
    }
}
