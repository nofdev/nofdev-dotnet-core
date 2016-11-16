using System;
using System.Collections.Generic;
using System.Reflection;
using Nofdev.Core.SOA;

namespace Nofdev.Client
{
    public interface IProxyStrategy
    {

        /// <summary>
        /// 获取远程请求地址
        /// </summary>
        /// <param name="interfaceType">代理的接口类</param>
        /// <param name="method">要调用的方法</param>
        /// <returns></returns>
        string GetRemoteUrl(Type interfaceType, MethodInfo method);
 

        /// <summary>
        /// 获取远程请求参数
        /// </summary>
        /// <param name="args">接口方法的请求参数</param>
        /// <returns></returns>
        IDictionary<string, string> GetParams(object[] args);

        /// <summary>
        /// 获取返回结果类
        /// </summary>
        /// <param name="method">要调用的方法</param>
        /// <param name="realReturnType">远程请求返回结果</param>
        /// <param name="httpMessageSimple"></param>
        /// <returns></returns>
        object GetResult(MethodInfo method, Type realReturnType, HttpMessageSimple httpMessageSimple);
    }
}