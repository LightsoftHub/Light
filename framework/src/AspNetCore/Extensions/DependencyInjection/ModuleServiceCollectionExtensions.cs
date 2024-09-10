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
        private static IEnumerable<T> GetClassesInheritFrom<T>(Assembly[] assemblies)
        {
            if (assemblies == null || assemblies.Length == 0)
            {
                // get from all assembly if not define assemblies to scan
                assemblies ??= AppDomain.CurrentDomain.GetAssemblies();
            }

            // get all classes inherit from type of T
            return assemblies
                .SelectMany(s => s.GetTypes())
                .Where(x =>
                    typeof(T).IsAssignableFrom(x)
                    && x.IsClass && !x.IsAbstract && !x.IsGenericType)
                .Select(s => (T)Activator.CreateInstance(s));
        }

        /// <summary>
        /// Scan & add module services with IConfiguration
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <param name="assemblies"></param>
        /// <returns></returns>
        public static IServiceCollection ScanModuleServices(this IServiceCollection services,
            IConfiguration configuration,
            params Assembly[] assemblies)
        {
            // get all classes inherit from interface
            var moduleServices = GetClassesInheritFrom<IModule>(assemblies);

            foreach (var instance in moduleServices)
            {
                instance?.ConfigureServices(services);
                instance?.ConfigureServices(services, configuration);
            }

            return services;
        }
    }
}