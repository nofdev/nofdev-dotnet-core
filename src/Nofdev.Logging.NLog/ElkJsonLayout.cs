using System.Collections.Generic;
using System.Dynamic;
using System.Reflection;
using System.Text;
using System.Threading;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using NLog;
using NLog.Layouts;
using Nofdev.Core.SOA;

namespace Nofdev.Logging.NLog
{
    /// <summary>
    /// ELK JSON layout,special format for search engine friendly
    /// </summary>
    [Layout(Name)]
    public class ElkJsonLayout : JsonLayout
    {
        internal const string Name = "ElkJsonLayout";
        public string Prefix { get; set; } = "~~~json~~~";

        protected override string GetFormattedMessage(LogEventInfo logEvent)
        {
            var sb = new StringBuilder();
            sb.Append(Prefix);

            dynamic log = new ExpandoObject();
            log.timestamp = logEvent.TimeStamp;
            log.level = logEvent.Level.Name;
            log.logger = logEvent.LoggerName;
            log.thread = Thread.CurrentThread.ManagedThreadId;
            log.message = logEvent.FormattedMessage;
            log.serviceContext = ServiceContext.Current;
            log.exception = logEvent.Exception;
            var settings = new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver()};

            if (logEvent.Parameters?.Length > 0)
            {
                var para = logEvent.Parameters[0];
             
                var paraType = para.GetType();
                if (paraType == typeof (string))
                {
                    sb.Append(para);
                }
                else
                {
                    var dict = log as IDictionary<string, object>;
                    foreach (var propertyInfo in paraType.GetProperties())
                    {
                        var key = propertyInfo.Name[0].ToString().ToLower() + propertyInfo.Name.Substring(1);
                        var val = propertyInfo.GetValue(para, null);
                        if (dict.ContainsKey(key))
                            dict[key] = val;
                        else
                        {
                            dict.Add(new KeyValuePair<string, object>(key, val));
                        }
                    }
                    sb.Append(JsonConvert.SerializeObject(dict, settings));
                }
               
            }
            else
                sb.Append(JsonConvert.SerializeObject(log, settings));
            return sb.ToString();
        }


    }
}