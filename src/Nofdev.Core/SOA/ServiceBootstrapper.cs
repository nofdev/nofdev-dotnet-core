using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices.ComTypes;
using Nofdev.Core.SOA.Annotations;
using Nofdev.Core.Util;

namespace Nofdev.Core.SOA
{
    public class ServiceBootstrapper
    {

        public ServiceBootstrapper()
        {
        }

        //private static ServiceBootstrapper _instance;
        //public static ServiceBootstrapper Instance => _instance ?? (_instance = new ServiceBootstrapper());

        public static ServiceRegistry UrlTypes { get; } = new ServiceRegistry();

        public Dictionary<string, List<Type>> UnmatchedInterfaces { get; } = new Dictionary<string, List<Type>>();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="assemblies"></param>
        /// <param name="iocTypes">types were registered in IoC container</param>
        public  void Scan(IEnumerable<Assembly> assemblies, ICollection<Type> iocTypes)
        {
            var types = Enum.GetNames(typeof(ServiceType)).ToList();
            foreach (var asm in assemblies)//.Where(a => a.GetCustomAttributes<BootstrapAttribute>().Any()))
            {
                types.ForEach(t =>
                {
                    Add(ScanByNameConvention(asm,t),t,iocTypes);
                });
                Add(Scan<FacadeServiceAttribute>(asm),Enum.GetName(typeof(ServiceType), ServiceType.Facade), iocTypes);
                Add(Scan<DomainServiceAttribute>(asm), Enum.GetName(typeof(ServiceType), ServiceType.Service), iocTypes);
                Add(Scan<MicroServiceAttribute>(asm), Enum.GetName(typeof(ServiceType), ServiceType.Micro), iocTypes);
            }
        }

        private  IEnumerable<Type> ScanByNameConvention(Assembly assembly,params string[] suffixes)
        {
            return
                ScanByNameConvention(assembly.GetTypes(),suffixes);
        }

        private IEnumerable<Type> ScanByNameConvention(IEnumerable<Type> types, params string[] suffixes)
        {
            return
                types.Where(t => t.GetTypeInfo().IsInterface && suffixes.Any(s => t.Name.EndsWith(s)));
        }

        private  void Add(IEnumerable<Type> types, string layer, ICollection<Type> excludedTypes)
        {
            foreach (var type in types)
            {
                var key = GetInterfaceKey(type, layer);

                if (!excludedTypes.Contains(type))
                {
                    if (!UnmatchedInterfaces.ContainsKey(layer))
                    {
                        UnmatchedInterfaces.Add(layer, new List<Type>());
                    }
                    if(!UnmatchedInterfaces[layer].Contains(type))
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
            return Scan<T>(assembly.GetTypes());
        }

        private IEnumerable<Type> Scan<T>(IEnumerable<Type> types) where T : ServiceDefinationAttribute
        {
            return
               types.Where(t => t.GetTypeInfo().IsInterface && t.GetTypeInfo().GetCustomAttribute<T>() != null);
        }
    }
}
