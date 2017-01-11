using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Nofdev.Gateway
{
    public class Startup : Server.Startup
           
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = CreateConfigurationBuilder(env);
            builder.AddJsonFile("rpc.json");

            if (env.IsDevelopment())
            {
                // For more details on using the user secret store see http://go.microsoft.com/fwlink/?LinkID=532709
                builder.AddUserSecrets();
            }
            Configuration = builder.Build();
        }


        protected IIdentityServerBuilder IdentityServerBuilder;

        protected virtual void ConfigureIdentityService<TDbContext, TUser, TRole>(IServiceCollection services, string connectionName = "DefaultConnection")
             where TDbContext : DbContext
            where TUser : IdentityUser
            where TRole : IdentityRole
        {
            services.AddDbContext<TDbContext>(options =>
                 options.UseSqlServer(Configuration.GetConnectionString(connectionName)));
            services.AddIdentity<TUser, TRole>()
              .AddEntityFrameworkStores<TDbContext>()
              .AddDefaultTokenProviders();
        }

        /// <summary>
        /// extend your own IdentityServer logic here,such as Client,Scope,and etc.
        /// </summary>
        /// <param name="services"></param>
        protected virtual IIdentityServerBuilder ConfigureIdentityServer(IServiceCollection services)
        {
            //identityServerBuilder.AddAspNetIdentity<TUser>();
          return  services.AddIdentityServer()
              .AddTemporarySigningCredential()
              .AddInMemoryPersistedGrants();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public override void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, IApplicationLifetime appLifetime)
        {

            base.Configure(app, env, loggerFactory, appLifetime);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
                app.UseBrowserLink();
            }
            //else
            //{
            //    app.UseExceptionHandler("/Home/Error");
            //}

            //app.UseStaticFiles();
            app.UseIdentity();
            app.UseIdentityServer();

            // Add external authentication middleware below. To configure them please see http://go.microsoft.com/fwlink/?LinkID=532715

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
