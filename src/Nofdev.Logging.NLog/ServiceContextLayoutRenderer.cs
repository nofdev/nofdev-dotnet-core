using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using NLog;
using NLog.LayoutRenderers;
using Nofdev.Core.SOA;

namespace Nofdev.Logging.NLog
{
    /// <summary>
    /// ServiceContextLayoutRenderer,the key is 'sc'
    /// </summary>
    [LayoutRenderer(Name)]
    public class ServiceContextLayoutRenderer : LayoutRenderer
    {
        internal const string Name = "sc";

        protected override void Append(StringBuilder builder, LogEventInfo logEvent)
        {
            builder.Append(ServiceContext.Current.ToJson());
        }
    }


    internal static class ServiceContextExtensions
    {
      
        public static string ToJson(this ServiceContext context)
        {
            var settings = new JsonSerializerSettings {ContractResolver = new CamelCasePropertyNamesContractResolver()};
            return JsonConvert.SerializeObject(context, settings);
        }
    }
}
