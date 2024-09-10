using Light.AspNetCore.Modules;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using System.Reflection;

namespace Light.AspNetCore.Builder;

public static class ModuleApplicationBuilderExtensions
{
    /// <summary>
    /// Scan & use module pipelines
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="configuration"></param>
    /// <param name="assemblies"></param>
    /// <returns></returns>
    public static IApplicationBuilder ScanModulePipelines(
        this IApplicationBuilder builder,
        IConfiguration configuration,
        params Assembly[] assemblies)
    {
        if (assemblies == null || assemblies.Length == 0)
        {
            // get from all assembly if not define assemblies to scan
            assemblies ??= AppDomain.CurrentDomain.GetAssemblies();
        }

        // get all classes inherit from interface
        var modulePipelines = assemblies
            .SelectMany(s => s.GetTypes())
            .Where(x =>
                typeof(IModulePipeline).IsAssignableFrom(x)
                && x.IsClass && !x.IsAbstract && !x.IsGenericType)
            .Select(s => Activator.CreateInstance(s) as IModulePipeline);

        foreach (var instance in modulePipelines)
        {
            instance?.ConfigurePipelines(builder);
            instance?.ConfigurePipelines(builder, configuration);
        }

        return builder;
    }

    /// <summary>
    /// Scan & use module job pipelines
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="configuration"></param>
    /// <param name="assemblies"></param>
    /// <returns></returns>
    public static IApplicationBuilder ScanModuleJobs(
        this IApplicationBuilder builder,
        IConfiguration configuration,
        params Assembly[] assemblies)
    {
        if (assemblies == null || assemblies.Length == 0)
        {
            // get from all assembly if not define assemblies to scan
            assemblies ??= AppDomain.CurrentDomain.GetAssemblies();
        }

        // get all classes inherit from interface
        var modulePipelines = assemblies
            .SelectMany(s => s.GetTypes())
            .Where(x =>
                typeof(IModuleJobPipeline).IsAssignableFrom(x)
                && x.IsClass && !x.IsAbstract && !x.IsGenericType)
            .Select(s => Activator.CreateInstance(s) as IModuleJobPipeline);

        foreach (var instance in modulePipelines)
        {
            instance?.UseJobPipelines(builder);
            instance?.UseJobPipelines(builder, configuration);
        }

        return builder;
    }
}
