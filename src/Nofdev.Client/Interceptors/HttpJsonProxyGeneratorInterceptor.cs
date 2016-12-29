using System;
using System.Reflection;
using System.Threading.Tasks;
using Castle.DynamicProxy;
using Microsoft.Extensions.Logging;
using Nofdev.Core.Util;

namespace Nofdev.Client.Interceptors
{
    public class HttpJsonProxyGeneratorInterceptor : IInterceptor
    {
        private readonly HttpJsonProxy _proxy;
        private readonly ILogger<HttpJsonProxyGeneratorInterceptor> _logger;

        public HttpJsonProxyGeneratorInterceptor(HttpJsonProxy proxy,ILogger<HttpJsonProxyGeneratorInterceptor> logger)
        {
            _proxy = proxy;
            _logger = logger;
        }

        public void Intercept(IInvocation invocation)
        {
            var returnType = invocation.Method.ReturnType;
            var returnTypeInfo = returnType.GetTypeInfo();
            var realType = invocation.Method.GetRealReturnType();
            var isTask = returnTypeInfo.IsGenericType && returnTypeInfo.BaseType == typeof(Task);

            _logger.LogDebug( $"Return type is Task<{realType}>");
           
            MakeRemoteCall(invocation, realType,isTask);

        }
         

        private   void MakeRemoteCall(IInvocation invocation, Type realType, bool isTask)
        {
            var returnValue = _proxy.GetType()
                .GetMethod("MakeRemoteCall")
                .MakeGenericMethod(realType)
                .Invoke(_proxy,
                    new object[]
                    {
                        invocation.Method.DeclaringType,
                        invocation.Method,
                        realType,
                        invocation.Arguments
                    });

            var task = (Task)returnValue;
            if (isTask)
            {
                invocation.ReturnValue = task;
                return;
            }

            try
            {
                task.Wait();

                if (task.IsFaulted)
                {
                    return;
                }
                var result =
                     task.GetType()
                         .GetProperty("Result")
                         .GetValue(task, null);

                invocation.ReturnValue = Convert.ChangeType(result, realType);
            }
            catch (Exception e)
            {
                _logger.LogError( $"Error when proxy to interface { _proxy.GetType()}",e);
                throw;
            }
        }
    }
}