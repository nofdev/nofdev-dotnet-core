using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Nofdev.Core.SOA.Annotations;
using Nofdev.Core.Util;

namespace Nofdev.Core.SOA
{
    public class ServiceBootstrapper
    {
        private readonly IServiceProvider _serviceProvider;

        public ServiceBootstrapper(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        //private static ServiceBootstrapper _instance;
        //public static ServiceBootstrapper Instance => _instance ?? (_instance = new ServiceBootstrapper());

        public static ServiceRegistry UrlTypes { get; } = new ServiceRegistry();

        public Dictionary<string, List<Type>> UnmatchedInterfaces { get; } = new Dictionary<string, List<Type>>();

        public  void Scan(IEnumerable<Assembly> assemblies)
        {
            var types = typeof (ServiceType).GetMembers().Select(m => m.Name).ToList();
            foreach (var asm in assemblies.Where(a => a.GetCustomAttributes<BootstrapAttribute>().Any()))
            {
                types.ForEach(t =>
                {
                    Add(ScanByNameConvention(asm,t),t);
                });
                Add(Scan<FacadeServiceAttribute>(asm),ServiceType.Facade.ToString());
                Add(Scan<DomainServiceAttribute>(asm), ServiceType.Service.ToString());
                Add(Scan<MicroServiceAttribute>(asm), ServiceType.Micro.ToString());
            }
        }

        private  IEnumerable<Type> ScanByNameConvention(Assembly assembly,params string[] suffixes)
        {
            return 
                assembly.GetTypes().Where(t => t.GetTypeInfo().IsInterface && suffixes.Any(s => t.Name.EndsWith(s)));
        }


        private  void Add(IEnumerable<Type> types, string layer)
        {
            foreach (var type in types)
            {
                var key = GetInterfaceKey(type, layer);

                if (_serviceProvider.GetService(type) == null)
                {
                    if (!UnmatchedInterfaces.ContainsKey(layer))
                    {
                        UnmatchedInterfaces.Add(layer,new List<Type>());
                    }
                    UnmatchedInterfaces[layer].Add(type);
                    continue;
                }

                if (!UrlTypes.ContainsKey(key))
                    UrlTypes.Add(key, type);
            }
        }


        protected  string GetInterfaceKey(Type t, string layer)
        {
            var ns = t.Namespace;
            var typeName = t.Name.RemovePrefixI();
            return $"{layer}.{ns}.{typeName}".ToLower();
        }

        private IEnumerable<Type> Scan<T>(Assembly assembly) where T : ServiceDefinationAttribute
        {
            return
               assembly.GetTypes().Where(t => t.GetTypeInfo().IsInterface && t.GetTypeInfo().GetCustomAttribute<T>() != null);
        }

   
    }
}
