using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using System.Text.RegularExpressions;
using Autofac;
using Autofac.Extras.DynamicProxy;
using Nofdev.Client.Interceptors;
using Nofdev.Core.SOA;

namespace Nofdev.AutofacExt
{
    public static class ContainerExtensions
    {
        public static void RegisterRemoteAPI(this ContainerBuilder  containerBuilder,IEnumerable<string> assemblyFiles, string regex = "(Facade|Service)$")
        {
            foreach (var file in assemblyFiles)
            {
                var asm = AssemblyLoadContext.Default.LoadFromAssemblyPath(file);
                var interfaces = asm.GetTypes().Where(t => t.IsInterface && Regex.IsMatch(t.Name, regex)).ToList();
                interfaces.ForEach(i =>
                {
                    containerBuilder.RegisterType(i).EnableInterfaceInterceptors().InterceptedBy(typeof(HttpJsonProxyGeneratorInterceptor));
                });
            }
        }

        public static void RegisterServices(this ContainerBuilder containerBuilder, string regex, params Assembly[] assemblies)
        {
            RegisterServices(containerBuilder.RegisterDefault, regex, assemblies);
        }

        public static void RegisterServices(this ContainerBuilder containerBuilder, string regex, Assembly assembly, Dictionary<Type, Type> depententTypes)
        {
            RegisterServices((interfaceType, implType) =>
            {
                containerBuilder.Register((c, t) =>
                {
                    var b = containerBuilder.RegisterType(implType).As(interfaceType);
                    depententTypes?.ToList().ForEach(pair =>
                    {
                        b.WithParameter(new TypedParameter(pair.Key, c.Resolve(pair.Value)));
                    });
                    return b;
                });
            }, regex, assembly);
        }

        public static void RegisterExposedServices(this ContainerBuilder containerBuilder, string layer, params Assembly[] assemblies)
        {
            RegisterServices((it, impl) =>
            {
                containerBuilder.RegisterType(impl).As(it);
                ServiceBootstrapper.Instance.Add(it, layer);
            }, layer + "$", assemblies);
        }

        public static void RegisterServices(Action<Type, Type> registerAction, string regex, params Assembly[] assemblies)
        {
            foreach (var assembly in assemblies)
            {
                var services = assembly.GetTypes()
                    .Where(t => t.IsClass && t.GetInterfaces().Length > 0 && Regex.IsMatch(t.Name, regex,
                                    RegexOptions.IgnoreCase | RegexOptions.Compiled)).ToList();
                services.ForEach(s =>
                {
                    var interfaces = s.GetInterfaces().ToList();
                    if (interfaces.Count == 1)
                    {
                        registerAction.Invoke(interfaces[0], s);
                        return;
                    }

                    interfaces = s.GetInterfaces().Where(i => i.Name == "I" + s.Name).ToList();
                    if (interfaces.Any())
                    {
                        registerAction.Invoke(interfaces[0], s);
                        return;
                    }

                    interfaces = s.GetInterfaces().Where(i => i.Name.Contains(s.Name)).ToList();
                    if (interfaces.Any())
                    {
                        registerAction.Invoke(interfaces[0], s);
                        return;
                    }

                    interfaces = s.GetInterfaces().Where(i => !i.IsGenericType).ToList();
                    if (interfaces.Any())
                    {
                        registerAction.Invoke(interfaces[0], s);
                        return;
                    }
                    registerAction.Invoke(s.GetInterfaces()[0], s);
                });
            }
        }

        private static void RegisterDefault(this ContainerBuilder containerBuilder, Type interfaceType, Type implType)
        {
            containerBuilder.RegisterType(implType).As(interfaceType).InstancePerLifetimeScope();
        }
    }
}
