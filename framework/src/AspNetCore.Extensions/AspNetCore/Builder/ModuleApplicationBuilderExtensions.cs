using Light.AspNetCore.Modularity;
using Microsoft.AspNetCore.Builder;
using System.Reflection;

namespace Light.AspNetCore.Builder;

public static class ModuleApplicationBuilderExtensions
{
    private static IApplicationBuilder ConfigureModulePipelines<T>(this IApplicationBuilder builder,
        params Assembly[] assemblies)
        where T : IModulePipeline
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
                typeof(T).IsAssignableFrom(x)
                && x.IsClass && !x.IsAbstract && !x.IsGenericType)
            .Select(s => Activator.CreateInstance(s) as IModulePipeline);

        foreach (var instance in modulePipelines)
        {
            instance?.ConfigurePipelines(builder);
        }

        return builder;
    }


    /// <summary>
    /// Scan & use module pipelines
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="configuration"></param>
    /// <param name="assemblies"></param>
    /// <returns></returns>
    public static IApplicationBuilder UseModules(this IApplicationBuilder builder,
        params Assembly[] assemblies)
    {
        return builder.ConfigureModulePipelines<ModulePipeline>(assemblies);
    }

    /// <summary>
    /// Scan & use module job pipelines
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="configuration"></param>
    /// <param name="assemblies"></param>
    /// <returns></returns>
    public static IApplicationBuilder ScanModuleJobs(this IApplicationBuilder builder,
        params Assembly[] assemblies)
    {
        return builder.ConfigureModulePipelines<ModuleJob>(assemblies);
    }
}
