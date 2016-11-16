using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Nofdev.Client
{
    public class ConfigManager
    {
        public static RpcConfig ReadRpcConfig(string root)
        {
            var rpcFile = Path.Combine(root, RpcConfig.FileName);
            if (!File.Exists(rpcFile))
                throw new FileNotFoundException($"{rpcFile} not found.");
            return ResolveJson<RpcConfig>(rpcFile);
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

       
    }
}