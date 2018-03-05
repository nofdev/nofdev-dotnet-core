using System.Collections.Specialized;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Nofdev.Core.SOA;

namespace Nofdev.Client
{
    public class ServiceContextJsonSerializer : IServiceContextSerializer
    {
        public const string KeyPrefix = "SERVICE-CONTEXT";
        #region Implementation of IServiceContextSerializer

        public NameValueCollection ToNameValueCollection(ServiceContext context)
        {
            var settings = new JsonSerializerSettings {ContractResolver = new CamelCasePropertyNamesContractResolver()};

            var nvc = new NameValueCollection();
            foreach (var item in context.GetItems())
            {
                nvc.Add(KeyPrefix + "-" + item.Key, JsonConvert.SerializeObject(item.Value, settings));
            }
            return nvc;
        }

        public ServiceContext Deserialize(NameValueCollection collection)
        {
            var settings = new JsonSerializerSettings {ContractResolver = new CamelCasePropertyNamesContractResolver()};
            var contextItems = ServiceContext.Current.GetItems();
            foreach (var key in collection.AllKeys)
            {
                if (!key.ToUpper().StartsWith(KeyPrefix))
                    continue;
                var dictKey = key.ToUpper().Replace(KeyPrefix + "-", "");
                if (!contextItems.ContainsKey(dictKey))
                    continue;
                var type = ServiceContext.Current[dictKey].GetType();
                ServiceContext.Current[dictKey] = JsonConvert.DeserializeObject(collection[key], type, settings);
            }
            return ServiceContext.Current;
        }

        #endregion
    }
}