using System;
using System.Reflection;
using System.Threading.Tasks;
using Castle.DynamicProxy;

namespace Nofdev.Client.Interceptors
{
    public class HttpJsonProxyGeneratorInterceptor : IInterceptor
    {
        //private static readonly ILogger logger = LogHelper.LoggerManager.GetLogger(typeof (HttpJsonProxyGeneratorInterceptor));
        private readonly HttpJsonProxy _proxy;

        public HttpJsonProxyGeneratorInterceptor(HttpJsonProxy proxy)
        {
            _proxy = proxy;
        }

        public void Intercept(IInvocation invocation)
        {
            //TODO need exception handling??
            //TODO what if { result: null, err: {...} } returned?

            var returnType = invocation.Method.ReturnType;
            var returnTypeInfo = returnType.GetTypeInfo();
            var realType = returnType;
            var isTask = false;
            if (returnTypeInfo.IsGenericType && returnTypeInfo.BaseType == typeof(Task))
            {
                realType = returnType.GenericTypeArguments[0];
                //logger.Debug(() => $"Return type is Task<{realType}>");
                isTask = true;
            }
            else if (returnType == typeof (void))
            {
                realType = typeof(object);
            }
            else
            {
                //logger.Debug(() => $"Return type is Task for underlying void returning type");
            }
            makeRemoteCall(invocation, realType,isTask);

           
        }
         

        private   void makeRemoteCall(IInvocation invocation, Type realType, bool isTask)
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

            //try
            //{
                task.Wait();

                if (task.IsFaulted)
                {
                    return; 
                }
                var result =
                     task.GetType()
                         .GetProperty("Result")
                         .GetValue(task, null);

                invocation.ReturnValue = Convert.ChangeType(result,realType);
            //}
            //catch (Exception e)
            //{
            //    //logger.Error(e,() => $"Error when proxy to interface { _proxy.GetType()}");
            //    throw e;
            //}
        }
    }
}