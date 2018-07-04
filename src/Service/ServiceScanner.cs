using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Nofdev.Core.SOA;
using Nofdev.Core.SOA.Annotations;
using Nofdev.Core.Util;

namespace Nofdev.Service
{
    public class ServiceScanner
    {
        protected ServiceScanner()
        {
            
        }

        private static ServiceScanner _instance;
        public static ServiceScanner Instance => _instance ?? (_instance = new ServiceScanner());

        public static ServiceRegistry UrlTypes { get; } = new ServiceRegistry();

  
        /// <summary>
        /// 
        /// </summary>
        /// <param name="assemblies"></param>
        public  void Scan(IEnumerable<Assembly> assemblies)
        {
            var types = Enum.GetNames(typeof(ServiceType)).ToList();
            foreach (var asm in assemblies)
            {
                types.ForEach(t =>
                {
                    Add(ScanByNameConvention(asm,t),t);
                });
                Add(Scan<FacadeServiceAttribute>(asm),Enum.GetName(typeof(ServiceType), ServiceType.Facade));
                Add(Scan<DomainServiceAttribute>(asm), Enum.GetName(typeof(ServiceType), ServiceType.Service));
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

        public void Add(IEnumerable<Type> types, string layer)
        {
            foreach (var type in types)
            {
               Add(type,layer);
            }
        }

        public void Add(Type type, string layer)
        {
            var key = GetInterfaceKey(type, layer);

            if (!UrlTypes.ContainsKey(key))
                UrlTypes.Add(key, type);
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

        public static Type GetServiceType(string packageName, string interfaceName, string serviceLayer)
        {
            var key =
                $"{serviceLayer}.{packageName.Replace('-', '.')}.{interfaceName}"
                    .ToLower();


            if (!UrlTypes.ContainsKey(key))
                key =
                    $"{serviceLayer}.{packageName.Replace('-', '.')}.{interfaceName}{serviceLayer}"
                        .ToLower();

            if (!UrlTypes.ContainsKey(key))
            {
                throw new InvalidOperationException($"Can not find interface {packageName}.{interfaceName} in {serviceLayer} layer.");
            }
            return UrlTypes[key];
        }

    }
}
