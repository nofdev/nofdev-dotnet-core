using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using Autofac;
using Autofac.Core;
using Castle.DynamicProxy;
using Nofdev.Client;
using Nofdev.Client.Interceptors;

namespace Nofdev.AutofacExt
{
    public class ServiceProxyBuilder
    {
        //private static readonly Dictionary<string, IContainer> containers = new Dictionary<string, IContainer>();
        //private static readonly Dictionary<string, IContainer> typeContainers = new Dictionary<string, IContainer>();
        //internal static readonly Dictionary<string, ServiceLocation> TypeLocations =  new Dictionary<string, ServiceLocation>();

        //public static T Resolve<T>()
        //{
        //    var key = typeof(T).FullName;
        //    return typeContainers[key].Resolve<T>();
        //}

        //public static object Resolve(Type type)
        //{
        //    var key = type.FullName;
        //    return typeContainers[key].Resolve(type);
        //}
        private readonly ContainerBuilder _containerBuilder;

        public ServiceProxyBuilder(ContainerBuilder containerBuilder)
        {
            _containerBuilder = containerBuilder;
            _containerBuilder.RegisterType<HttpJsonProxyGeneratorInterceptor>();
            _containerBuilder.RegisterType<HttpJsonProxy>();
        }

        public void RegisterServiceByNameConvention(ServiceLocation service)
        {
            RegisterServiceByNameConvention<DefaultProxyStrategy>(service);
        }

        public void RegisterServiceByNameConvention<TStrategy>(ServiceLocation service) where TStrategy : IProxyStrategy
        {
            var asm = Assembly.Load(service.AssemblyString);
            var serviceInterfaceTypes =
                asm.GetTypes()
                    .Where(
                        type =>
                            type.IsInterface &&
                            Regex.IsMatch(type.Name, service.TypeNameScan,
                                RegexOptions.IgnoreCase | RegexOptions.Compiled)
                    ).ToList();
            Register<TStrategy>(service, serviceInterfaceTypes);
        }

        public void RegisterService<T>(ServiceLocation service) where T : Attribute
        {
            RegisterService<T, DefaultProxyStrategy>(service);
        }

        public void RegisterService<T, TStrategy>(ServiceLocation service) where T : Attribute
            where TStrategy : IProxyStrategy
        {
            var asm = Assembly.Load(service.AssemblyString);
            var serviceInterfaceTypes = asm.GetTypes().Where(type =>
                type.IsInterface && type.GetCustomAttributes().Any(attr => attr.GetType() == typeof(T))
            );
            Register<TStrategy>(service, serviceInterfaceTypes);
        }

        /// <summary>
        /// </summary>
        /// <typeparam name="TStrategy"></typeparam>
        /// <param name="location"></param>
        /// <param name="serviceInterfaceTypes"></param>
        /// <seealso
        ///     cref="https://stackoverflow.com/questions/21073858/how-register-an-interface-that-has-no-implentation-with-autofac" />
        public void Register<TStrategy>(ServiceLocation location, IEnumerable<Type> serviceInterfaceTypes)
            where TStrategy : IProxyStrategy
        {
            _containerBuilder.RegisterType<TStrategy>().As<IProxyStrategy>();

            foreach (var serviceInterfaceType in serviceInterfaceTypes)
                _containerBuilder.Register(c =>
                {
                    var proxyGen = new ProxyGenerator();
                    return proxyGen.CreateInterfaceProxyWithoutTarget(serviceInterfaceType,
                        c.Resolve<HttpJsonProxyGeneratorInterceptor>(new ResolvedParameter(
                                (pi, ctx) => pi.ParameterType == typeof(HttpJsonProxy),
                                (pi, ctx) => ctx.Resolve<HttpJsonProxy>(new ResolvedParameter(
                                    (pi2, ctx2) => pi2.ParameterType == typeof(IProxyStrategy),
                                    (pi2, ctx2) =>
                                        ctx2.Resolve<IProxyStrategy>(new NamedParameter("baseURL", location.BaseUrl))
                                ))
                            )
                        ));
                }).As(serviceInterfaceType);
        }
    }
}