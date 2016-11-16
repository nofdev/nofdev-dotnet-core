using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Autofac.Extras.DynamicProxy.Core;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using Nofdev.Core.Dependency;
using Nofdev.Core.SOA;
using Nofdev.Core.Util;
using Nofdev.Client.Interceptors;

namespace Nofdev.Server
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();

            _assemblies = ComponentScan.GetAssemblies(env.ContentRootPath).ToList();
        }

        public IConfigurationRoot Configuration { get; protected set; }
        public IContainer ApplicationContainer { get; protected set; }
        private readonly List<Assembly> _assemblies; 

        // This method gets called by the runtime. Use this method to add services to the container.
        public virtual IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();


            var builder = new ContainerBuilder();
            DependencyBootstrapper.Scan(_assemblies, (i, t) =>
            {
                builder.RegisterType(t).As(i);
            });

            builder.Populate(services);

            var sp = new AutofacServiceProvider(builder.Build());
            var sb = new ServiceBootstrapper(sp);
            sb.Scan(_assemblies);
            sb.UnmatchedInterfaces.Values.ToList().ForEach(v =>
            {
                v.ForEach(t =>
                {
                    builder.RegisterType(t)
                        .AsImplementedInterfaces()
                        .InstancePerLifetimeScope()
                        .EnableInterfaceInterceptors()
                        .InterceptedBy(typeof (HttpJsonProxyGeneratorInterceptor));
                });
            });

            ApplicationContainer = builder.Build();
            sp = new AutofacServiceProvider(ApplicationContainer);

            return sp;
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public virtual void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, IApplicationLifetime appLifetime)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();
            loggerFactory.AddNLog();

            env.ConfigureNLog("nlog.config");

            app.UseMvc();

            appLifetime.ApplicationStopped.Register(() => this.ApplicationContainer.Dispose());
        }
    }
}
