using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Nofdev.Client.Interceptors;

namespace Nofdev.Client
{
    public class ServiceProxyBuilder
    {
        private readonly IWindsorContainer _container;
        private static readonly Dictionary<string,IWindsorContainer> containers = new Dictionary<string, IWindsorContainer>();
        private static readonly Dictionary<string,IWindsorContainer> typeContainers = new Dictionary<string, IWindsorContainer>(); 
        internal static readonly Dictionary<string,ServiceLocation> TypeLocations = new Dictionary<string, ServiceLocation>();

        public ServiceProxyBuilder(IWindsorContainer container)
        {
            _container = container;

            _container.Register(
                         Component.For<HttpJsonProxyGeneratorInterceptor>(),
                         Component.For<HttpJsonProxy>()
                         );
        }

        public static T Resolve<T>()
        {
            var key = typeof (T).FullName;
            return typeContainers[key].Resolve<T>();
        }

        public static object Resolve(Type type)
        {
            var key = type.FullName;
            return typeContainers[key].Resolve(type);
        }

        public void RegisterAssembliesByNameConvention(RpcConfig config)
        {
            RegisterAssembliesByNameConvention<DefaultProxyStrategy>(config);
        }

        public void RegisterAssembliesByNameConvention<TStrategy>(RpcConfig config) where TStrategy : IProxyStrategy
        {
            foreach (var service in config.Services.Where(m => !string.IsNullOrWhiteSpace(m.TypeNameScan)))
            {
                var asm = Assembly.Load(service.AssemblyString);
                var serviceInterfaceTypes =
                    asm.GetTypes()
                        .Where(
                            type =>
                                type.IsInterface &&
                                Regex.IsMatch(type.Name, service.TypeNameScan,
                                    RegexOptions.IgnoreCase | RegexOptions.Compiled)
                                && !typeContainers.ContainsKey(type.FullName)
                        ).ToList();
                Register<TStrategy>(service, serviceInterfaceTypes);

                serviceInterfaceTypes.ForEach(m => TypeLocations.Add(m.FullName,service));
            }
        }

        public void RegisterAssemblies<T>(RpcConfig config) where T : Attribute
        {
            RegisterAssemblies<T,DefaultProxyStrategy>(config);
        }

        public void RegisterAssemblies<T,TStrategy>(RpcConfig config) where T : Attribute
            where TStrategy : IProxyStrategy
        {

            foreach (var service in config.Services)
            {
                var asm = Assembly.Load(service.AssemblyString);
                var serviceInterfaceTypes = asm.GetTypes().Where(type => type.IsInterface &&  type.GetCustomAttributes().Any(attr => attr.GetType() == typeof(T)) && !typeContainers.ContainsKey(type.FullName));
                Register<TStrategy>(service, serviceInterfaceTypes);
            }
        }

        public void Register<TStrategy>(ServiceLocation service, IEnumerable<Type> serviceInterfaceTypes) 
            where TStrategy : IProxyStrategy
        {
            var serviceKey = $"{service.AssemblyName}@{service.BaseUrl}";

            var containerKey = serviceKey + ":" + typeof (TStrategy).FullName;
            IWindsorContainer c;
            if (!containers.ContainsKey(containerKey))
            {
                c = new WindsorContainer();
                _container.AddChildContainer(c);


                c.Register(
                    Component.For<HttpJsonProxyGeneratorInterceptor>(),
                    Component.For<HttpJsonProxy>()
                    );

                c.Register(
                    Component.For<IProxyStrategy>()
                        .ImplementedBy<TStrategy>()
                        .DependsOn(Dependency.OnValue<string>(service.BaseUrl))
                    );
                containers.Add(containerKey, c);
            }
            else
            {
                // c = _container.GetChildContainer(serviceKey);
                c = containers[containerKey];
            }


            foreach (var serviceInterfaceType in serviceInterfaceTypes)
            {
                var key = serviceInterfaceType.FullName;
                c.Register(
                    Component.For(serviceInterfaceType)
                        .Interceptors<HttpJsonProxyGeneratorInterceptor>()
                    );
                typeContainers.Add(key, c);

                //_container.Register(Component.For(serviceInterfaceType)
                //    .Interceptors<RemoteApiProxyInterceptor>());

                _container.Register(Component.For(serviceInterfaceType).Instance(Resolve(serviceInterfaceType)));
            }
        }


    }
}
