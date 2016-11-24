using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Nofdev.Core.SOA;
using Nofdev.Core.SOA.Annotations;

namespace Nofdev.Client
{
    public class Bootstrap
    {
        public static RpcConfig RpcConfig { get; set; }
        protected static bool AutoLoadToIoC = false;

        public static void Startup(string root)
        {
            RpcConfig = ConfigManager.ReadRpcConfig(root);
        }

        internal static readonly Dictionary<string, ServiceLocation> TypeLocations = new Dictionary<string, ServiceLocation>();

        public static ServiceLocation GetServiceLocationByType(Type type)
        {
            var key = type.FullName;
            if (TypeLocations.ContainsKey(key))
                return TypeLocations[key];
            var ns = type.Namespace;
            while (true)
            {
                foreach (var location in RpcConfig.Services)
                {
                    if (location.Namespace == ns)
                    {

                        location.Layer = Enum.GetName(typeof(ServiceType), GetServiceType(type)).ToLower();
                        TypeLocations.Add(key, location);
                        return location;
                    }
                }

                if (ns.LastIndexOf('.') < 0)
                    break;

                ns = ns.Substring(0, ns.LastIndexOf('.')).TrimEnd('.');
            }

            throw new NotSupportedException($"cann't find the service location of type:{type.FullName}");
        }

        public static ServiceType GetServiceType(Type type)
        {
            var typeName = type.Name;
            var members = typeof (ServiceType).GetMembers().Select(m => m.Name).ToList();
            foreach (var m in members)
            {
                if (typeName.EndsWith(m))
                {
                    return (ServiceType)Enum.Parse(typeof (ServiceType), m);
                }
            }
           if(type.GetTypeInfo().GetCustomAttributes<MicroServiceAttribute>().Any())
                return ServiceType.Micro;
            if (type.GetTypeInfo().GetCustomAttributes<DomainServiceAttribute>().Any())
                return ServiceType.Service;
           return ServiceType.Facade;
        }
    }

}
