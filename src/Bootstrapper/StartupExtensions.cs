using System;
using System.Reflection;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Nofdev.Bootstrapper.AutofacExt;
using Nofdev.Service;

namespace Nofdev.Bootstrapper
{
    public static class StartupExtensions
    {
        public static ContainerBuilder ContainerBuilder { get; } = new ContainerBuilder();

        public static IServiceProvider AddNofdev(this IServiceCollection services, ServiceScanOptions options)
        {
            ContainerBuilder.RegisterURF();
            ContainerBuilder.RegisterServices(options);
            ContainerBuilder.Populate(services);
            return new AutofacServiceProvider(ContainerBuilder.Build());
        }

        public static IServiceCollection AddRepository<T>(this IServiceCollection services, Assembly assembly,
            Action<DbContextOptionsBuilder> action) where T : DbContext
        {
            services.AddDbContext<T>(action);
            services.AddTransient<DbContext, T>();
            ContainerBuilder.RegisterRepositories<T>(assembly);
            return services;
        }

        public static void UseDbContextMigration(this IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                foreach (var context in serviceScope.ServiceProvider.GetServices<DbContext>())
                    context.Database.Migrate();
            }
        }
    }
}