using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Nofdev.Core.SOA;
using Nofdev.Core.SOA.Annotations;

namespace Nofdev.Client
{
    public class ConfigManager
    {
        private static RpcConfig RpcConfig { get; set; }

        public static RpcConfig ReadRpcConfig(string root)
        {
            var rpcFile = Path.Combine(root, RpcConfig.FileName);
            if (!File.Exists(rpcFile))
                return null;
            RpcConfig = ResolveJson<RpcConfig>(rpcFile);
            return RpcConfig;
        }

        private static T ResolveJson<T>(string file)
        {
            var settings = new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() };
            using (var sr = new StreamReader(new FileStream(file,FileMode.Open)))
            {
                var text = sr.ReadToEnd();
                return JsonConvert.DeserializeObject<T>(text,settings);
            }
        }

        static readonly Dictionary<string, ServiceLocation> typeLocations = new Dictionary<string, ServiceLocation>();



        public static ServiceLocation GetServiceLocationByType(Type type)
        {
            var key = type.FullName;
            if (typeLocations.ContainsKey(key))
                return typeLocations[key];
            var ns = type.Namespace;
            while (true)
            {
                foreach (var location in RpcConfig.Services)
                {
                    if (location.Namespace == ns)
                    {

                        location.Layer = Enum.GetName(typeof(ServiceType), GetServiceType(type)).ToLower();
                        typeLocations.Add(key, location);
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
            var members = typeof(ServiceType).GetMembers().Select(m => m.Name).ToList();
            foreach (var m in members)
            {
                if (typeName.EndsWith(m))
                {
                    return (ServiceType)Enum.Parse(typeof(ServiceType), m);
                }
            }
            if (type.GetTypeInfo().GetCustomAttributes<DomainServiceAttribute>().Any())
                return ServiceType.Service;
            return ServiceType.Facade;
        }
    }
}