using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Nofdev.Service.Util
{
    public class Invoker
    {
        public static async Task<object> Invoke(Type serviceType, object service, string methodName, string @params)
        {
            object val = null;

            var methods = serviceType.GetTypeInfo().GetMethods();
            var method = methods.FirstOrDefault(m =>
                string.Compare(m.Name, methodName, StringComparison.CurrentCultureIgnoreCase) == 0);
            if (method != null)
            {
                //async task
                if (method.ReturnType.GetTypeInfo().BaseType == typeof(Task))
                {
                    if (@params != null && "null" != @params)
                    {
                        var task =
                            (Task)
                            method.Invoke(service,
                                (await DeserializeAsync(@params,
                                    method.GetParameters().Select(p => p.ParameterType).ToArray())).ToArray());
                        await task.ContinueWith(it =>
                        {
                            dynamic dtask = it;
                            val = dtask.Result;
                        });
                    }
                    else
                    {
                        var task = (Task) method.Invoke(service, null);
                        await task.ContinueWith(it =>
                        {
                            dynamic dtask = it;
                            val = dtask.Result;
                        });
                    }
                }
                else
                {
                    if (@params != null && "null" != @params)
                    {
                        val = method.Invoke(service,
                            Deserialize(@params, method.GetParameters().Select(p => p.ParameterType).ToArray())
                                .ToArray());
                    }
                    else
                    {
                        val = method.Invoke(service, null);
                    }
                }
            }
            else
            {
                throw new InvalidOperationException(
                    $"Can not find method {methodName} of interface type {serviceType}.");
            }


            return val;
        }


        static IEnumerable<object> Deserialize(string rawParams, Type[] paramTypes)
        {
            var array = JArray.Parse(rawParams);
            return array.Select((item, i) => item.ToObject(paramTypes[i]));
        }

        static async Task<IEnumerable<object>> DeserializeAsync(string rawParams, Type[] paramTypes)
        {
            var array = JArray.Parse(rawParams);
            return await Task.Run(() => array.Select((item, i) => item.ToObject(paramTypes[i])));
        }
    }
}