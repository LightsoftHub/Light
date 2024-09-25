using Light.AspNetCore.Modularity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Reflection;

namespace Light.Extensions.DependencyInjection
{
    public static class ModuleServiceCollectionExtensions
    {
        /// <summary>
        /// Scan & add module services with IConfiguration
        /// </summary>
        public static IServiceCollection AddModules<T>(this IServiceCollection services,
            IConfiguration configuration,
            bool includeJobs = false,
            params Assembly[] assemblies)
            where T : LightModule
        {
            if (assemblies == null || assemblies.Length == 0)
            {
                // get from all assembly if not define assemblies to scan
                assemblies ??= AppDomain.CurrentDomain.GetAssemblies();
            }

            // get all classes inherit from interface
            var moduleServices = assemblies
                .SelectMany(s => s.GetTypes())
                .Where(x =>
                    typeof(T).IsAssignableFrom(x)
                    && x.IsClass && !x.IsAbstract && !x.IsGenericType)
                .Select(s => Activator.CreateInstance(s) as IModule);

            foreach (var instance in moduleServices)
            {
                instance?.ConfigureServices(services);
                instance?.ConfigureServices(services, configuration);

                if (includeJobs)
                {
                    instance?.ConfigureJobs(services);
                    instance?.ConfigureJobs(services, configuration);
                }
            }

            return services;
        }

        /// <summary>
        /// Scan & add module services with IConfiguration
        /// </summary>
        public static IServiceCollection AddModules(this IServiceCollection services,
            IConfiguration configuration,
            bool includeJobs = false,
            params Assembly[] assemblies) =>
            services.AddModules<LightModule>(configuration, includeJobs, assemblies);
    }
}