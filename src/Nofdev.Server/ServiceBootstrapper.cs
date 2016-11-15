using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Nofdev.Core.SOA;
using Nofdev.Core.SOA.Annotations;

namespace Nofdev.Server
{
    public class ServiceBootstrapper
    {
        protected ServiceBootstrapper()
        {
            
        }

        private static ServiceBootstrapper _instance;
        public static ServiceBootstrapper Instance => _instance ?? (_instance = new ServiceBootstrapper());

        public Dictionary<string, Type> UrlTypes { get; } = new Dictionary<string, Type>();

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
                if (!UrlTypes.ContainsKey(key))
                    UrlTypes.Add(key, type);
            }
        }


        protected  string GetInterfaceKey(Type t, string layer)
        {
            var ns = t.Namespace;
            var typeName = RemovePrefixI(t.Name);
            return $"{layer}.{ns}.{typeName}".ToLower();
        }

        private IEnumerable<Type> Scan<T>(Assembly assembly) where T : ServiceDefinationAttribute
        {
            return
               assembly.GetTypes().Where(t => t.GetTypeInfo().IsInterface && t.GetTypeInfo().GetCustomAttribute<T>() != null);
        }

        public static string RemovePrefixI( string name)
        {
            if (name.Length > 1 && name[0] == 'I' && char.IsUpper(name[1]))
                return name.Substring(1);
            return name;
        }
    }
}
