using Light.AspNetCore.Modularity.Pipelines;
using Microsoft.AspNetCore.Builder;
using System.Reflection;

namespace Light.AspNetCore.Builder;

public static class ModuleApplicationBuilderExtensions
{
    /// <summary>
    /// Scan & use module pipelines
    /// </summary>
    public static IApplicationBuilder UseModules<T>(this IApplicationBuilder builder,
        bool initializeJobs = false,
        params Assembly[] assemblies)
        where T : LightModule
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

            if (initializeJobs)
            {
                instance?.InitializeJobs(builder);
            }
        }

        return builder;
    }


    /// <summary>
    /// Scan & use module pipelines
    /// </summary>
    public static IApplicationBuilder UseModules(this IApplicationBuilder builder,
        bool includeJobs = false,
        params Assembly[] assemblies)
    {
        return builder.UseModules<LightModule>(includeJobs, assemblies);
    }
}
