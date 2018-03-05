using System;
using System.Collections.Generic;
using System.Linq;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Castle.Windsor.MsDependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Nofdev.Client;
using Nofdev.Client.Interceptors;
using Nofdev.Core.Dependency;
using Nofdev.Core.SOA;
using Nofdev.Service.Util;

namespace Nofdev.Service
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Castle.Windsor.MsDependencyInjection"/>
    /// <remarks>https://github.com/volosoft/castle-windsor-ms-adapter</remarks>
    public static class StartupExtensions
    {
      
        public static IServiceProvider AddNofdev(this IServiceCollection services, string root, ServiceScanSettings settings)
        {
            var container = new WindsorContainer();
            var sp = WindsorRegistrationHelper.CreateServiceProvider(container, services);
            container.RegisterRpcServices(root);
            //container.Register(
            //    Component.For<ILogger>().UsingFactory<LoggerFactory,>()
            //    Component.For<ComponentScan>() //todo:log
            //);
            var cs = new ComponentScan(sp.GetService<ILogger<ComponentScan>>());
            var assemblies = cs.GetAssemblies(root, settings).ToList();
            var registeredTypes = new List<Type>();
            DependencyBootstrapper.Scan(assemblies, (i, t) =>
            {
                container.Register(Component.For(i).ImplementedBy(t).LifestyleTransient());
                registeredTypes.Add(i);
            });

            var sb = new ServiceBootstrapper();
            sb.Scan(assemblies, registeredTypes);
            sb.UnmatchedInterfaces.Values.SelectMany(m => m).ToList().ForEach(t =>
            {
                container.Register(Component.For(t).Interceptors<HttpJsonProxyGeneratorInterceptor>().LifestyleTransient());
            });
            return sp;
        }
        
    }
}
