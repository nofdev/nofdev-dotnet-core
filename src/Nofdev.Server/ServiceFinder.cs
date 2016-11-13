using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Nofdev.Core.SOA.Annotations;
using Nofdev.Core.Util;

namespace Nofdev.Server
{
    public class ServiceFinder
    {
        /*
        private readonly ServiceConfig _serviceConfig;

        public ServiceFinder(ServiceConfig serviceConfig)
        {
            _serviceConfig = serviceConfig;
        }

        public Dictionary<string, Type> UrlTypes { get; } = new Dictionary<string, Type>();

        public Dictionary<string, Type> Scan()
        {
            _serviceConfig.Modules.ToList().ForEach(m =>
            {
                var physicalPath = GetPhysicalPath(m.Path);
                Add(ComponentScan.GetTypes<FacadeServiceAttribute>(physicalPath), m, FacadeServiceAttribute.UrlSegment);
                Add(ComponentScan.GetTypes<DomainServiceAttribute>(physicalPath), m, DomainServiceAttribute.UrlSegment);
                Add(ComponentScan.GetTypes<MicroServiceAttribute>(physicalPath), m, MicroServiceAttribute.UrlSegment);
                Add(ComponentScan.GetTypes(physicalPath, m.TypeNameScan), m);
            });
            return UrlTypes;
        }

        private void Add(Dictionary<string, Type[]> dict, Module m, string layer = null)
        {
            foreach (var types in dict.Values)
            {
                foreach (var type in types.Where(t => t.IsInterface))
                {
                    var key = GetInterfaceKey(type, m, layer);
                    if (!UrlTypes.ContainsKey(key))
                        UrlTypes.Add(key, type);
                }
            }
        }

        protected string GetPhysicalPath(string path)
        {
            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, path);
        }



        protected string GetInterfaceKey(Type t, Module module, string layer = null)
        {
            var ns = t.Namespace;
            var typeName = t.Name.RemovePrefixI();
            return $"{layer ?? module.ServiceLayer}.{ns}.{typeName}.{module.Protocol}".ToLower();
        }

    */
    }
}