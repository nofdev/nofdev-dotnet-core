using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Text;
using Newtonsoft.Json;
using Nofdev.Core;
using Nofdev.Core.SOA;
using Nofdev.Core.Util;

namespace Nofdev.Client
{
    public class DefaultProxyStrategy : IProxyStrategy
    {
        //private static readonly ILogger logger = LogHelper.LoggerManager.GetLogger(typeof (DefaultProxyStrategy));
     
        public string GetRemoteUrl(Type interfaceType, MethodInfo method)
        {
            //logger.Debug(() => $"The baseUrl is {baseURL}");
            var location = Bootstrap.GetServiceLocationByType(interfaceType);
            string serviceLayer = location.Layer;
            string interName = interfaceType.Name.RemovePrefixI();
            var sb = new StringBuilder();
            sb.Append(location.BaseUrl);
            sb.Append("/");
            sb.Append(serviceLayer);
            sb.AppendFormat("/{0}/",location.Protocol);
            var moduleName = interfaceType.Namespace;
            moduleName = moduleName.Replace('.', '-');
            sb.Append(moduleName);
            sb.Append("/");
            sb.Append(interName);
            sb.Append("/");
            sb.Append(method.Name);
            var remoteURL = sb.ToString();
            //logger.Debug(() => $"The remoteUrl is {remoteURL}");
            return remoteURL;
        }

        public IDictionary<string, string> GetParams(object[] args)
        {
            var dictParams = new Dictionary<string, string>();

            var paramsStr = JsonConvert.SerializeObject(args);
            //logger.Debug(() => $"The params string is {paramsStr}");
            dictParams["params"] = paramsStr;
            return dictParams;
        }

        public object GetResult(MethodInfo method, Type realReturnType, HttpMessageSimple httpMessageSimple)
        {
            if (httpMessageSimple.StatusCode != HttpStatusCode.OK.GetHashCode())
            {
                throw new HttpRequestException(httpMessageSimple.Body);
            }

            var result = httpMessageSimple.Body;
            //logger.Debug(() => $"The request return {result}");
            //logger.Debug(() => $"The method real return type is {realReturnType}");

            var type = typeof (HttpJsonResponse<>).MakeGenericType(realReturnType);
            dynamic httpJsonResponse = JsonConvert.DeserializeObject(result, type);

            if (httpJsonResponse.err == null)
            {
                return httpJsonResponse.val;
            }
            throw ExceptionUtil.GetExceptionInstance(httpJsonResponse.err as ExceptionMessage);
        }
    }

}