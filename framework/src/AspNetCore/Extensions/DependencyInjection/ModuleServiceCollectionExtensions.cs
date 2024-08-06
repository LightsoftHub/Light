using Light.AspNetCore.Modules;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Light.Extensions.DependencyInjection
{
    public static class ModuleServiceCollectionExtensions
    {
        /// <summary>
        /// Scan & configure all module services
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <param name="assemblies"></param>
        /// <returns></returns>
        public static IServiceCollection AutoConfigureModuleServices(
            this IServiceCollection services,
            IConfiguration configuration,
            IEnumerable<Assembly> assemblies)
        {
            // get from all assembly if not define assemblies to scan
            assemblies ??= AppDomain.CurrentDomain.GetAssemblies();

            // get all classes inherit from interface
            var moduleServices = assemblies
                .SelectMany(s => s.GetTypes())
                .Where(x =>
                    typeof(IModuleServiceCollection).IsAssignableFrom(x)
                    && x.IsClass && !x.IsAbstract && !x.IsGenericType)
                .Select(s => Activator.CreateInstance(s) as IModuleServiceCollection);

            foreach (var instance in moduleServices)
            {
                instance?.ConfigureServices(services, configuration);
            }

            return services;
        }

        /// <summary>
        /// Scan & configure all module services
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <param name="assemblies"></param>
        /// <returns></returns>
        public static IServiceCollection AutoConfigureModuleServices(
            this IServiceCollection services,
            IConfiguration configuration,
            params Assembly[] assemblies)
        {
            if (assemblies is null || assemblies.Length == 0)
                assemblies = AppDomain.CurrentDomain.GetAssemblies();

            services.AutoConfigureModuleServices(configuration, assemblies.AsEnumerable());

            return services;
        }
    }
}