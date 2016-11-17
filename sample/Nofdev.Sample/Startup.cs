using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Nofdev.Sample
{
    public class Startup : Nofdev.Server.Startup
    {
        public Startup(IHostingEnvironment env) : base(env)
        {
           
        }

        #region Overrides of Startup

        public override void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory,
            IApplicationLifetime appLifetime)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            base.Configure(app, env, loggerFactory, appLifetime);

            //app.UseMvc(r =>
            //{
            //    r.MapRoute(
            //           name: "facade",
            //           template: "json/facade",
            //           defaults: new { controller = "Facade", action = "Json" }
            //       );
            //    r.MapRoute(
            //          name: "service",
            //          template: "json/service",
            //          defaults: new { controller = "Service", action = "Json" }
            //      );
            //    r.MapRoute(
            //          name: "micro",
            //          template: "json/micro",
            //          defaults: new { controller = "Micro", action = "Json" }
            //      );
            //});
        }

        public override IServiceProvider ConfigureServices(IServiceCollection services)
        {
            return base.ConfigureServices(services);
        }

        #endregion
    }

   
}
