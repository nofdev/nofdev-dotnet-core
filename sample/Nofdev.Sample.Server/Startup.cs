using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Nofdev.Sample.Server
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

        }
        #endregion
    }

   
}
