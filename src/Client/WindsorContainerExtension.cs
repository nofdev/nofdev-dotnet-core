using Castle.Windsor;
using Nofdev.Core.SOA.Annotations;

namespace Nofdev.Client
{
    public static class WindsorContainerExtension
    {

        public static void RegisterRpcServices(this IWindsorContainer container, string root)
        {
            RegisterRpcServices<DefaultProxyStrategy>(container,root);
        }

        public static void RegisterRpcServices<TStrategy>(this IWindsorContainer container,string root) where TStrategy : IProxyStrategy
        {
            var config = ConfigManager.ReadRpcConfig(root);
            if(config == null) return;
            container.RegisterRpcServices<TStrategy>(config);
        }

        public static void RegisterRpcServices<TStrategy>(this IWindsorContainer container,RpcConfig config) where TStrategy : IProxyStrategy
        {
            var serviceProxyBuilder = new ServiceProxyBuilder(container);
            serviceProxyBuilder.RegisterAssemblies<FacadeServiceAttribute, TStrategy>(config);
            serviceProxyBuilder.RegisterAssemblies<DomainServiceAttribute, TStrategy>(config);
            serviceProxyBuilder.RegisterAssembliesByNameConvention<TStrategy>(config);
        }
    }
}
