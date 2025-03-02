﻿using Light.AspNetCore.Modularity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Light.Extensions.DependencyInjection;

public static class ModuleJobServiceCollectionExtensions
{
    /// <summary>
    /// Scan & add module jobs services with IConfiguration
    /// </summary>
    public static IServiceCollection AddModuleJobs<T>(this IServiceCollection services,
        IConfiguration configuration,
        Assembly[] assemblies)
        where T : ModuleJob
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
            .Select(s => Activator.CreateInstance(s) as ModuleJob);

        foreach (var instance in moduleServices)
        {
            instance?.Add(services);
            instance?.Add(services, configuration);
        }

        return services;
    }

    /// <summary>
    /// Scan & add module jobs services with IConfiguration
    /// </summary>
    public static IServiceCollection AddModuleJobs(this IServiceCollection services,
        IConfiguration configuration,
        Assembly[] assemblies) =>
        services.AddModuleJobs<ModuleJob>(configuration, assemblies);
}
