using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Nofdev.Gateway.Data;
using Nofdev.Gateway.Models;

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


        protected override void AddServices(IServiceCollection services)
        {
            ConfigureDbContext(services);

            ConfigureIdentity(services);

            services.AddMvc();

            // Add application services.
            //services.AddTransient<IEmailSender, AuthMessageSender>();
            //services.AddTransient<ISmsSender, AuthMessageSender>();


            IdentityServerBuilder = services.AddIdentityServer()
              .AddTemporarySigningCredential()
              .AddInMemoryPersistedGrants();

            ConfigureIdentityServer(IdentityServerBuilder);
        }


        protected virtual void ConfigureDbContext(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                 options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

        }

        protected virtual void ConfigureIdentity(IServiceCollection services)
        {
            services.AddIdentity<ApplicationUser, IdentityRole>()
              .AddEntityFrameworkStores<ApplicationDbContext>()
              .AddDefaultTokenProviders();
        }

        /// <summary>
        /// extend your own IdentityServer logic here,such as Client,Scope,and etc.
        /// </summary>
        /// <param name="identityServerBuilder"></param>
        protected virtual void ConfigureIdentityServer(IIdentityServerBuilder identityServerBuilder)
        {
            identityServerBuilder.AddAspNetIdentity<ApplicationUser>();
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
