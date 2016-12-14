using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Autofac.Extras.DynamicProxy.Core;
using Castle.DynamicProxy;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using Nofdev.Client;
using Nofdev.Core.Dependency;
using Nofdev.Core.SOA;
using Nofdev.Core.Util;
using Nofdev.Client.Interceptors;

namespace Nofdev.Server
{
    /*
     * reference:《ASP.NET Core 整合Autofac和Castle实现自动AOP拦截》http://www.cnblogs.com/maxzhang1985/p/5919936.html
     */
    public class Startup
    {
        protected Startup()
        {
            
        }

        public Startup(IHostingEnvironment env)
        {
            var builder = CreateConfigurationBuilder(env);
            Configuration = builder.Build();

            Assemblies = ComponentScan.GetAssemblies(env.ContentRootPath).ToList();

            Nofdev.Client.Bootstrap.Startup(env.ContentRootPath);

        }

        protected  IConfigurationBuilder CreateConfigurationBuilder(IHostingEnvironment env)
        {
           var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            return builder;
        }


        public IConfigurationRoot Configuration { get; protected set; }
        public IContainer ApplicationContainer { get; protected set; }
        protected  List<Assembly> Assemblies; 

        // This method gets called by the runtime. Use this method to add services to the container.
        public  IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            AddServices(services);
            FilterAssemblies();
            return  RegisterIoC(services);
        }

        protected virtual void AddServices(IServiceCollection services)
        {
            
        }

        protected virtual void FilterAssemblies()
        {
            
        }

        protected virtual IServiceProvider RegisterIoC(IServiceCollection services)
        {

            var builder = new ContainerBuilder();
            builder.RegisterType<HttpJsonProxy>();
            builder.RegisterType<HttpJsonProxyGeneratorInterceptor>();

            var registeredTypes = new List<Type>();
            DependencyBootstrapper.Scan(Assemblies, (i, t) =>
            {
                builder.RegisterType(t).As(i);
                registeredTypes.Add(i);
            });



            var sb = new ServiceBootstrapper();
            sb.Scan(Assemblies, registeredTypes);
            sb.UnmatchedInterfaces.Values.ToList().ForEach(v =>
            {
                v.ForEach(t =>
                {
                    builder.RegisterInstance(GenerateProxy(t)).As(t)
                        //.AsImplementedInterfaces()
                        //.InstancePerLifetimeScope()
                        .EnableInterfaceInterceptors()
                        .InterceptedBy(typeof(HttpJsonProxyGeneratorInterceptor));
                });
            });

            builder.Populate(services);
            ApplicationContainer = builder.Build();

            var sp = new AutofacServiceProvider(ApplicationContainer);
            return sp;
        }

        protected object GenerateProxy(Type type)
        {
            return new ProxyGenerator().CreateInterfaceProxyWithoutTarget(type);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public virtual  void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, IApplicationLifetime appLifetime)
        {

            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();
            loggerFactory.AddNLog();

            app.UseMvc();
            appLifetime.ApplicationStopped.Register(() => this.ApplicationContainer.Dispose());
        }

    }
}
