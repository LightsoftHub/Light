using Light.AspNetCore.Modularity;
using Microsoft.AspNetCore.Builder;
using System.Reflection;

namespace Light.AspNetCore.Builder;

public static class ModuleJobBuilderExtensions
{
    /// <summary>
    /// Scan & configure module jobs pipelines
    /// </summary>
    public static IApplicationBuilder UseModuleJobs<T>(this IApplicationBuilder builder, Assembly[] assemblies)
        where T : ModuleJob
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
            .Select(s => Activator.CreateInstance(s) as ModuleJob);

        foreach (var instance in modulePipelines)
        {
            instance?.Initialize(builder);
        }

        return builder;
    }

    /// <summary>
    /// Scan & configure module jobs pipelines
    /// </summary>
    public static IApplicationBuilder UseModuleJobs(this IApplicationBuilder builder, Assembly[] assemblies)
    {
        return builder.UseModuleJobs<ModuleJob>(assemblies);
    }
}
