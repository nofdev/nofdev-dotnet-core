using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using Nofdev.Core.SOA;

namespace Nofdev.Client
{
    public class HttpJsonProxy
    {
        //private static readonly ILogger logger = LogHelper.LoggerManager.GetLogger(typeof (HttpJsonProxy));

        public HttpJsonProxy(IProxyStrategy strategy)
        {
            ProxyStrategy = strategy;
        }

        public IProxyStrategy ProxyStrategy { get; set; }

        public async Task<T> MakeRemoteCall<T>(Type inter, MethodInfo method, Type realReturnType, object[] args)
        {
            if (ProxyStrategy == null)
            {
                throw new InvalidOperationException("ProxyStrategy is not configured.");
            }
            var remoteUrl = "";
            try
            {

                bool isneedargs = args.Length > 0;
                remoteUrl = ProxyStrategy.GetRemoteUrl(inter, method);
                var paras = isneedargs?ProxyStrategy.GetParams(args):null;
                using (var hc = new HttpClient())
                {
                    ServiceContext.Current.CallId = new CallId();
                    var items = new ServiceContextJsonSerializer().ToNameValueCollection(ServiceContext.Current);
                    foreach (var key in items.AllKeys)
                    {
                        hc.DefaultRequestHeaders.Add(key.ToLower(), items[key]);
                    }
                    var response = await postAsync(hc, remoteUrl, paras);
                    return (T)ProxyStrategy.GetResult(method, realReturnType, new HttpMessageSimple
                    {
                        Body = response.Item1,
                        ContentType = response.Item2,
                        StatusCode = response.Item3
                    });
                }
            }
            catch (Exception e)
            {
                //logger.Error(e,() => $"Error when proxy to {remoteUrl} interface {inter}, method {method}");
                throw;
            }
        }

        private async Task<Tuple<string, string, int>> postAsync(HttpClient hc, string remoteUrl,
            IDictionary<string, string> paras)
        {

            if (paras == null)
            {
                paras = new Dictionary<string, string>();
            }

                var httpResponseMessage = await hc.PostAsync(remoteUrl, new FormUrlEncodedContent(paras)).ConfigureAwait(false);
            var content = await httpResponseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);
            var contentType = httpResponseMessage.Content.Headers.ContentType;
            var httpStatusCode = httpResponseMessage.StatusCode;
            return new Tuple<string, string, int>(content, contentType.MediaType, (int) httpStatusCode);
        }
    }
}