using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Loader;
using System.Text;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Nofdev.Service;

namespace Nofdev.AutofacExt
{
    public static class StartupExtensions
    {
        public static IServiceProvider AddNofdev(this IServiceCollection services, ContainerBuilder containerBuilder, ServiceScanOptions options)
        {
            var root = AppContext.BaseDirectory;
            if (options.RpcAssemblyFiles != null)
                containerBuilder.RegisterRemoteAPI(options.RpcAssemblyFiles.Select(f => Path.Combine(root, f)));
            options.ServiceModules.ToList().ForEach(m =>
            {
                m.Dependencies?.ToList().ForEach(d =>
                {
                    containerBuilder.RegisterServices(d.NameRegex, AssemblyLoadContext.Default.LoadFromAssemblyPath(Path.Combine(root, d.AssemblyFile)));
                });
                containerBuilder.RegisterExposedServices(m.Layer, AssemblyLoadContext.Default.LoadFromAssemblyPath(Path.Combine(root, m.EntryAssemblyFile)));

            });
            containerBuilder.Populate(services);
            return new AutofacServiceProvider(containerBuilder.Build());
        }
    }
}
