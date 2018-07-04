using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;

namespace Nofdev.Bootstrapper.AutoMapperExt
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddAutoMapper(this IServiceCollection services,
            IEnumerable<MappingAssemblyPair> assemblyPairs)
        {
            var profile = AutoProfileLoader.Load(assemblyPairs);
            Mapper.Initialize(x => x.AddProfile(profile));

            var openTypes = new[]
            {
                typeof(IValueResolver<,,>),
                typeof(IMemberValueResolver<,,,>),
                typeof(ITypeConverter<,>),
                typeof(IMappingAction<,>)
            };

            var allTypes = AutoProfileLoader.Assemblies
                .Where(a => a.GetName().Name != nameof(AutoMapper))
                .SelectMany(a => a.DefinedTypes)
                .ToArray();

            foreach (var type in openTypes.SelectMany(openType => allTypes
                .Where(t => t.IsClass
                            && !t.IsAbstract
                            && t.AsType().ImplementsGenericInterface(openType))))
                services.AddTransient(type.AsType());
            services.AddSingleton(Mapper.Configuration);

            return services.AddScoped<IMapper>(sp =>
                new Mapper(sp.GetRequiredService<IConfigurationProvider>(), sp.GetService));
        }

        private static bool ImplementsGenericInterface(this Type type, Type interfaceType)
        {
            return type.IsGenericType(interfaceType) || type.GetTypeInfo().ImplementedInterfaces
                       .Any(@interface => @interface.IsGenericType(interfaceType));
        }


        private static bool IsGenericType(this Type type, Type genericType)
        {
            return type.GetTypeInfo().IsGenericType && type.GetGenericTypeDefinition() == genericType;
        }
    }
}