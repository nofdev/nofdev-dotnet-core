using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Text;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Nofdev.Core;
using Nofdev.Core.SOA;
using Nofdev.Core.Util;

namespace Nofdev.Client
{
    public class DefaultProxyStrategy : IProxyStrategy
    {
        private readonly ILogger<DefaultProxyStrategy> _logger;

        private static readonly string[] ServiceLayerPostfixes = { "facade",  "service" };
        public const string Protocol = "json";
        private readonly string _baseUrl;

        public DefaultProxyStrategy(string baseURL, ILogger<DefaultProxyStrategy> logger)
        {
            _baseUrl = baseURL;
            _logger = logger;
        }

        protected Tuple<string, string> GetServiceInfo(Type inter)
        {
            if (ServiceProxyBuilder.TypeLocations.ContainsKey(inter.FullName))
            {
                var s = ServiceProxyBuilder.TypeLocations[inter.FullName];
                return new Tuple<string, string>(s.Layer, inter.Name.RemovePrefixI());
            }
            foreach (var postfix in ServiceLayerPostfixes)
            {
                if (inter.Name.EndsWith(postfix, StringComparison.CurrentCultureIgnoreCase))
                    return new Tuple<string, string>(postfix, inter.Name.Substring(0, inter.Name.Length - postfix.Length).RemovePrefixI());
            }
            throw new NotSupportedException(inter.FullName + " is not a supported service type.");
        }



        public string GetRemoteUrl(Type inter, MethodInfo method)
        {
            _logger.LogDebug($"The baseUrl is {_baseUrl}");
            var serviceInfo = GetServiceInfo(inter);
            string serviceLayer = serviceInfo.Item1;
            string interName = serviceInfo.Item2;
            var sb = new StringBuilder();
            sb.Append(_baseUrl);
            sb.Append("/");
            sb.Append(serviceLayer);
            sb.AppendFormat("/{0}/", Protocol);
            var moduleName = inter.Namespace;
            sb.Append(moduleName);
            sb.Append("/");
            sb.Append(interName);
            sb.Append("/");
            sb.Append(method.Name);
            var remoteURL = sb.ToString();
            _logger.LogDebug($"The remoteUrl is {remoteURL}");
            Console.WriteLine(remoteURL);
            return remoteURL;
        }

        public IDictionary<string, string> GetParams(object[] args)
        {
            var dictParams = new Dictionary<string, string>();

            var paramsStr = JsonConvert.SerializeObject(args);
            _logger.LogDebug($"The params string is {paramsStr}");
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
            _logger.LogDebug($"The request return {result}");
            _logger.LogDebug($"The method real return type is {realReturnType}");

            var type = typeof(HttpJsonResponse<>).MakeGenericType(realReturnType);
            dynamic httpJsonResponse = JsonConvert.DeserializeObject(result, type);

            if (httpJsonResponse.err == null)
            {
                return httpJsonResponse.val;
            }
            throw ExceptionUtil.GetExceptionInstance(httpJsonResponse.err as ExceptionMessage);
        }
    }

}