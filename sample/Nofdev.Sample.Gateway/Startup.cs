using System.Collections.Generic;
using IdentityServer4.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nofdev.Gateway.Data;
using Nofdev.Gateway.Models;
using Nofdev.Multitenancy.Identity.EntityFramework;

namespace Nofdev.Sample.Gateway
{
    public class Startup : Nofdev.Gateway.Startup
    {
        public Startup(IHostingEnvironment env) : base(env)
        {
        }

        #region Overrides of Startup

        protected override void ConfigureIdentityServer(IIdentityServerBuilder identityServerBuilder)
        {
            base.ConfigureIdentityServer(identityServerBuilder);
            identityServerBuilder.AddInMemoryClients(Config.GetClients())
                .AddInMemoryScopes(Config.GetScopes())
                .AddAspNetIdentity<ApplicationUser>();

        }

        protected override void ConfigureDbContext(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
        }


        protected override void ConfigureIdentity(IServiceCollection services)
        {
            services.AddIdentity<ApplicationUser, MultitenancyIdentityRole>()
              .AddEntityFrameworkStores<ApplicationDbContext>()
              .AddDefaultTokenProviders();
        }


        #endregion
    }
}
