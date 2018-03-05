﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Autofac;
using Autofac.Builder;
using Autofac.Core;
using Autofac.Features.Scanning;
using Castle.DynamicProxy;
using Castle.DynamicProxy.Internal;

namespace Nofdev.Service.DynamicProxy
{
    /// <summary>
    /// Adds registration syntax to the <see cref="ContainerBuilder"/> type.
    /// </summary>
    public static class RegistrationExtensions
    {
        private const string InterceptorsPropertyName = "Autofac.Extras.DynamicProxy.RegistrationExtensions.InterceptorsPropertyName";

        private static readonly IEnumerable<Autofac.Core.Service> EmptyServices = new Autofac.Core.Service[0];

        private static readonly ProxyGenerator ProxyGenerator = new ProxyGenerator();

        /// <summary>
        /// Enable class interception on the target type. Interceptors will be determined
        /// via Intercept attributes on the class or added with InterceptedBy().
        /// Only virtual methods can be intercepted this way.
        /// </summary>
        /// <typeparam name="TLimit">Registration limit type.</typeparam>
        /// <typeparam name="TRegistrationStyle">Registration style.</typeparam>
        /// <param name="registration">Registration to apply interception to.</param>
        /// <returns>Registration builder allowing the registration to be configured.</returns>
        public static IRegistrationBuilder<TLimit, ScanningActivatorData, TRegistrationStyle> EnableClassInterceptors<TLimit, TRegistrationStyle>(
            this IRegistrationBuilder<TLimit, ScanningActivatorData, TRegistrationStyle> registration)
        {
            return EnableClassInterceptors(registration, ProxyGenerationOptions.Default);
        }

        /// <summary>
        /// Enable class interception on the target type. Interceptors will be determined
        /// via Intercept attributes on the class or added with InterceptedBy().
        /// Only virtual methods can be intercepted this way.
        /// </summary>
        /// <typeparam name="TLimit">Registration limit type.</typeparam>
        /// <typeparam name="TRegistrationStyle">Registration style.</typeparam>
        /// <typeparam name="TConcreteReflectionActivatorData">Activator data type.</typeparam>
        /// <param name="registration">Registration to apply interception to.</param>
        /// <returns>Registration builder allowing the registration to be configured.</returns>
        public static IRegistrationBuilder<TLimit, TConcreteReflectionActivatorData, TRegistrationStyle> EnableClassInterceptors<TLimit, TConcreteReflectionActivatorData, TRegistrationStyle>(
            this IRegistrationBuilder<TLimit, TConcreteReflectionActivatorData, TRegistrationStyle> registration)
            where TConcreteReflectionActivatorData : ConcreteReflectionActivatorData
        {
            return EnableClassInterceptors(registration, ProxyGenerationOptions.Default);
        }

        /// <summary>
        /// Enable class interception on the target type. Interceptors will be determined
        /// via Intercept attributes on the class or added with InterceptedBy().
        /// Only virtual methods can be intercepted this way.
        /// </summary>
        /// <typeparam name="TLimit">Registration limit type.</typeparam>
        /// <typeparam name="TRegistrationStyle">Registration style.</typeparam>
        /// <param name="registration">Registration to apply interception to.</param>
        /// <param name="options">Proxy generation options to apply.</param>
        /// <param name="additionalInterfaces">Additional interface types. Calls to their members will be proxied as well.</param>
        /// <returns>Registration builder allowing the registration to be configured.</returns>
        public static IRegistrationBuilder<TLimit, ScanningActivatorData, TRegistrationStyle> EnableClassInterceptors<TLimit, TRegistrationStyle>(
            this IRegistrationBuilder<TLimit, ScanningActivatorData, TRegistrationStyle> registration,
            ProxyGenerationOptions options,
            params Type[] additionalInterfaces)
        {
            if (registration == null)
            {
                throw new ArgumentNullException(nameof(registration));
            }

            registration.ActivatorData.ConfigurationActions.Add((t, rb) => rb.EnableClassInterceptors(options, additionalInterfaces));
            return registration;
        }

        /// <summary>
        /// Enable class interception on the target type. Interceptors will be determined
        /// via Intercept attributes on the class or added with InterceptedBy().
        /// Only virtual methods can be intercepted this way.
        /// </summary>
        /// <typeparam name="TLimit">Registration limit type.</typeparam>
        /// <typeparam name="TRegistrationStyle">Registration style.</typeparam>
        /// <typeparam name="TConcreteReflectionActivatorData">Activator data type.</typeparam>
        /// <param name="registration">Registration to apply interception to.</param>
        /// <param name="options">Proxy generation options to apply.</param>
        /// <param name="additionalInterfaces">Additional interface types. Calls to their members will be proxied as well.</param>
        /// <returns>Registration builder allowing the registration to be configured.</returns>
        public static IRegistrationBuilder<TLimit, TConcreteReflectionActivatorData, TRegistrationStyle> EnableClassInterceptors<TLimit, TConcreteReflectionActivatorData, TRegistrationStyle>(
            this IRegistrationBuilder<TLimit, TConcreteReflectionActivatorData, TRegistrationStyle> registration,
            ProxyGenerationOptions options,
            params Type[] additionalInterfaces)
            where TConcreteReflectionActivatorData : ConcreteReflectionActivatorData
        {
            if (registration == null)
            {
                throw new ArgumentNullException(nameof(registration));
            }

            registration.ActivatorData.ImplementationType =
            ProxyGenerator.ProxyBuilder.CreateClassProxyType(
                registration.ActivatorData.ImplementationType, additionalInterfaces ?? new Type[0],
                options);

            registration.OnPreparing(e =>
            {
                var proxyParameters = new List<Parameter>();
                int index = 0;

                if (options.HasMixins)
                {
                    foreach (var mixin in options.MixinData.Mixins)
                    {
                        proxyParameters.Add(new PositionalParameter(index++, mixin));
                    }
                }

                proxyParameters.Add(new PositionalParameter(index++, GetInterceptorServices(e.Component, registration.ActivatorData.ImplementationType)
                    .Select(s => e.Context.ResolveService(s))
                    .Cast<IInterceptor>()
                    .ToArray()));

                if (options.Selector != null)
                {
                    proxyParameters.Add(new PositionalParameter(index, options.Selector));
                }

                e.Parameters = proxyParameters.Concat(e.Parameters).ToArray();
            });

            return registration;
        }

        /// <summary>
        /// Enable interface interception on the target type. Interceptors will be determined
        /// via Intercept attributes on the class or interface, or added with InterceptedBy() calls.
        /// </summary>
        /// <typeparam name="TLimit">Registration limit type.</typeparam>
        /// <typeparam name="TActivatorData">Activator data type.</typeparam>
        /// <typeparam name="TSingleRegistrationStyle">Registration style.</typeparam>
        /// <param name="registration">Registration to apply interception to.</param>
        /// <returns>Registration builder allowing the registration to be configured.</returns>
        public static IRegistrationBuilder<TLimit, TActivatorData, TSingleRegistrationStyle> EnableInterfaceInterceptors<TLimit, TActivatorData, TSingleRegistrationStyle>(
            this IRegistrationBuilder<TLimit, TActivatorData, TSingleRegistrationStyle> registration)
        {
            return EnableInterfaceInterceptors(registration, null);
        }

        /// <summary>
        /// Enable interface interception on the target type. Interceptors will be determined
        /// via Intercept attributes on the class or interface, or added with InterceptedBy() calls.
        /// </summary>
        /// <typeparam name="TLimit">Registration limit type.</typeparam>
        /// <typeparam name="TActivatorData">Activator data type.</typeparam>
        /// <typeparam name="TSingleRegistrationStyle">Registration style.</typeparam>
        /// <param name="registration">Registration to apply interception to.</param>
        /// <param name="options">Proxy generation options to apply.</param>
        /// <returns>Registration builder allowing the registration to be configured.</returns>
        public static IRegistrationBuilder<TLimit, TActivatorData, TSingleRegistrationStyle> EnableInterfaceInterceptors<TLimit, TActivatorData, TSingleRegistrationStyle>(
            this IRegistrationBuilder<TLimit, TActivatorData, TSingleRegistrationStyle> registration, ProxyGenerationOptions options)
        {
            if (registration == null)
            {
                throw new ArgumentNullException(nameof(registration));
            }

            registration.RegistrationData.ActivatingHandlers.Add((sender, e) =>
            {
                EnsureInterfaceInterceptionApplies(e.Component);

                var proxiedInterfaces = e.Instance.GetType().GetTypeInfo().GetInterfaces()
                .Where(i => i.GetTypeInfo().IsVisible || i.GetTypeInfo().Assembly.IsInternalToDynamicProxy()).ToArray();

                if (!proxiedInterfaces.Any())
                {
                    return;
                }

                var theInterface = proxiedInterfaces.First();
                var interfaces = proxiedInterfaces.Skip(1).ToArray();

                var interceptors = GetInterceptorServices(e.Component, e.Instance.GetType())
                .Select(s => e.Context.ResolveService(s))
                .Cast<IInterceptor>()
                .ToArray();


                e.Instance = options == null
                ? ProxyGenerator.CreateInterfaceProxyWithTarget(theInterface, null, e.Instance, interceptors)
                : ProxyGenerator.CreateInterfaceProxyWithTarget(theInterface, null, e.Instance, options, interceptors);
            });

            return registration;
        }

        /// <summary>
        /// Allows a list of interceptor services to be assigned to the registration.
        /// </summary>
        /// <typeparam name="TLimit">Registration limit type.</typeparam>
        /// <typeparam name="TActivatorData">Activator data type.</typeparam>
        /// <typeparam name="TStyle">Registration style.</typeparam>
        /// <param name="builder">Registration to apply interception to.</param>
        /// <param name="interceptorServices">The interceptor services.</param>
        /// <returns>Registration builder allowing the registration to be configured.</returns>
        /// <exception cref="System.ArgumentNullException">builder or interceptorServices</exception>
        public static IRegistrationBuilder<TLimit, TActivatorData, TStyle> InterceptedBy<TLimit, TActivatorData, TStyle>(
            this IRegistrationBuilder<TLimit, TActivatorData, TStyle> builder,
            params Autofac.Core.Service[] interceptorServices)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            if (interceptorServices == null || interceptorServices.Any(s => s == null))
            {
                throw new ArgumentNullException(nameof(interceptorServices));
            }

            object existing;
            if (builder.RegistrationData.Metadata.TryGetValue(InterceptorsPropertyName, out existing))
            {
                builder.RegistrationData.Metadata[InterceptorsPropertyName] =
               ((IEnumerable<Autofac.Core.Service>)existing).Concat(interceptorServices).Distinct();
            }
            else
            {
                builder.RegistrationData.Metadata.Add(InterceptorsPropertyName, interceptorServices);
            }

            return builder;
        }

        /// <summary>
        /// Allows a list of interceptor services to be assigned to the registration.
        /// </summary>
        /// <typeparam name="TLimit">Registration limit type.</typeparam>
        /// <typeparam name="TActivatorData">Activator data type.</typeparam>
        /// <typeparam name="TStyle">Registration style.</typeparam>
        /// <param name="builder">Registration to apply interception to.</param>
        /// <param name="interceptorServiceNames">The names of the interceptor services.</param>
        /// <returns>Registration builder allowing the registration to be configured.</returns>
        /// <exception cref="System.ArgumentNullException">builder or interceptorServices</exception>
        public static IRegistrationBuilder<TLimit, TActivatorData, TStyle> InterceptedBy<TLimit, TActivatorData, TStyle>(
            this IRegistrationBuilder<TLimit, TActivatorData, TStyle> builder,
            params string[] interceptorServiceNames)
        {
            if (interceptorServiceNames == null || interceptorServiceNames.Any(n => n == null))
            {
                throw new ArgumentNullException(nameof(interceptorServiceNames));
            }

            return InterceptedBy(builder, interceptorServiceNames.Select(n => new KeyedService(n, typeof(IInterceptor))).ToArray());
        }

        /// <summary>
        /// Allows a list of interceptor services to be assigned to the registration.
        /// </summary>
        /// <typeparam name="TLimit">Registration limit type.</typeparam>
        /// <typeparam name="TActivatorData">Activator data type.</typeparam>
        /// <typeparam name="TStyle">Registration style.</typeparam>
        /// <param name="builder">Registration to apply interception to.</param>
        /// <param name="interceptorServiceTypes">The types of the interceptor services.</param>
        /// <returns>Registration builder allowing the registration to be configured.</returns>
        /// <exception cref="System.ArgumentNullException">builder or interceptorServices</exception>
        public static IRegistrationBuilder<TLimit, TActivatorData, TStyle> InterceptedBy<TLimit, TActivatorData, TStyle>(
            this IRegistrationBuilder<TLimit, TActivatorData, TStyle> builder,
            params Type[] interceptorServiceTypes)
        {
            if (interceptorServiceTypes == null || interceptorServiceTypes.Any(t => t == null))
            {
                throw new ArgumentNullException(nameof(interceptorServiceTypes));
            }

            return InterceptedBy(builder, interceptorServiceTypes.Select(t => new TypedService(t)).ToArray());
        }

        /// <summary>
        /// Intercepts the interface of a transparent proxy (such as WCF channel factory based clients).
        /// </summary>
        /// <typeparam name="TLimit">Registration limit type.</typeparam>
        /// <typeparam name="TActivatorData">Activator data type.</typeparam>
        /// <typeparam name="TSingleRegistrationStyle">Registration style.</typeparam>
        /// <param name="registration">Registration to apply interception to.</param>
        /// <param name="additionalInterfacesToProxy">Additional interface types. Calls to their members will be proxied as well.</param>
        /// <returns>Registration builder allowing the registration to be configured.</returns>
        [Obsolete("dotnet core中暂时没有remoting类库，不能使用")]
        public static IRegistrationBuilder<TLimit, TActivatorData, TSingleRegistrationStyle> InterceptTransparentProxy<TLimit, TActivatorData, TSingleRegistrationStyle>(
            this IRegistrationBuilder<TLimit, TActivatorData, TSingleRegistrationStyle> registration, params Type[] additionalInterfacesToProxy)
        {
            return InterceptTransparentProxy(registration, null, additionalInterfacesToProxy);
        }

        /// <summary>
        /// Intercepts the interface of a transparent proxy (such as WCF channel factory based clients).
        /// </summary>
        /// <typeparam name="TLimit">Registration limit type.</typeparam>
        /// <typeparam name="TActivatorData">Activator data type.</typeparam>
        /// <typeparam name="TSingleRegistrationStyle">Registration style.</typeparam>
        /// <param name="registration">Registration to apply interception to.</param>
        /// <param name="options">Proxy generation options to apply.</param>
        /// <param name="additionalInterfacesToProxy">Additional interface types. Calls to their members will be proxied as well.</param>
        /// <returns>Registration builder allowing the registration to be configured.</returns>
        [Obsolete("dotnet core中暂时没有remoting类库，不能使用")]
        public static IRegistrationBuilder<TLimit, TActivatorData, TSingleRegistrationStyle> InterceptTransparentProxy<TLimit, TActivatorData, TSingleRegistrationStyle>(
            this IRegistrationBuilder<TLimit, TActivatorData, TSingleRegistrationStyle> registration, ProxyGenerationOptions options, params Type[] additionalInterfacesToProxy)
        {
            if (registration == null)
            {
                throw new ArgumentNullException(nameof(registration));
            }
            //TODO:加入core的remoting引用
            #region 暂时没有remoting类库引用

            //registration.RegistrationData.ActivatingHandlers.Add((sender, e) =>
            //{
            //    EnsureInterfaceInterceptionApplies(e.Component);

            //    if (!RemotingServices.IsTransparentProxy(e.Instance))
            //    {
            //        throw new DependencyResolutionException(string.Format(
            //        CultureInfo.CurrentCulture, RegistrationExtensionsResources.TypeIsNotTransparentProxy, e.Instance.GetType().FullName));
            //    }

            //    if (!e.Instance.GetType().GetTypeInfo().IsInterface)
            //    {
            //        throw new DependencyResolutionException(string.Format(
            //        CultureInfo.CurrentCulture, RegistrationExtensionsResources.TransparentProxyIsNotInterface, e.Instance.GetType().FullName));
            //    }

            //    if (additionalInterfacesToProxy.Any())
            //    {
            //        var remotingTypeInfo = (IRemotingTypeInfo)RemotingServices.GetRealProxy(e.Instance);

            //        var invalidInterfaces = additionalInterfacesToProxy
            //        .Where(i => !remotingTypeInfo.CanCastTo(i, e.Instance))
            //        .ToArray();

            //        if (invalidInterfaces.Any())
            //        {
            //            string message = string.Format(CultureInfo.CurrentCulture, RegistrationExtensionsResources.InterfaceNotSupportedByTransparentProxy,
            //                                                                                                                string.Join(", ", invalidInterfaces.Select(i => i.FullName)));
            //            throw new DependencyResolutionException(message);
            //        }
            //    }

            //    var interceptors = GetInterceptorServices(e.Component, e.Instance.GetType())
            //        .Select(s => e.Context.ResolveService(s))
            //        .Cast<IInterceptor>()
            //        .ToArray();

            //    e.Instance = options == null
            //    ? ProxyGenerator.CreateInterfaceProxyWithTargetInterface(e.Instance.GetType(), additionalInterfacesToProxy, e.Instance, interceptors)
            //    : ProxyGenerator.CreateInterfaceProxyWithTargetInterface(e.Instance.GetType(), additionalInterfacesToProxy, e.Instance, options, interceptors);
            //});

            #endregion

            return registration;
        }

        private static void EnsureInterfaceInterceptionApplies(IComponentRegistration componentRegistration)
        {
            if (componentRegistration.Services
            .OfType<IServiceWithType>()
            .Any(swt => !swt.ServiceType.GetTypeInfo().IsInterface || (!swt.ServiceType.GetTypeInfo().Assembly.IsInternalToDynamicProxy() && !swt.ServiceType.GetTypeInfo().IsVisible)))
            {
                throw new InvalidOperationException("interface proxy only supports interface services.");
            }
        }

        private static IEnumerable<Autofac.Core.Service> GetInterceptorServices(IComponentRegistration registration, Type implType)
        {
            if (registration == null)
            {
                throw new ArgumentNullException(nameof(registration));
            }

            if (implType == null)
            {
                throw new ArgumentNullException(nameof(implType));
            }

            var result = EmptyServices;

            object services;
            if (registration.Metadata.TryGetValue(InterceptorsPropertyName, out services))
            {
                result = result.Concat((IEnumerable<Autofac.Core.Service>)services);
            }

            if (implType.GetTypeInfo().IsClass)
            {
                result = result.Concat(implType
                    .GetTypeInfo()
                    .GetCustomAttributes(typeof(InterceptAttribute), true)
                    .Cast<InterceptAttribute>()
                    .Select(att => att.InterceptorService));

                result = result.Concat(implType
                    .GetInterfaces()
                    .SelectMany(i => i.GetTypeInfo().GetCustomAttributes(typeof(InterceptAttribute), true))
                    .Cast<InterceptAttribute>()
                    .Select(att => att.InterceptorService));
            }

            return result.ToArray();
        }
    }
}
