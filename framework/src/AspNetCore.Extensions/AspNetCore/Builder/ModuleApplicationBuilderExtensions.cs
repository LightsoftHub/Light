using Light.AspNetCore.Modularity;
using Microsoft.AspNetCore.Builder;
using System.Reflection;

namespace Light.AspNetCore.Builder;

public static class ModuleApplicationBuilderExtensions
{
    /// <summary>
    /// Scan & use module pipelines
    /// </summary>
    public static IApplicationBuilder UseModules<T>(this IApplicationBuilder builder,
        bool includeJobs = false,
        params Assembly[] assemblies)
        where T : ModulePipeline
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
            instance?.Use(builder);

            if (includeJobs)
            {
                instance?.UseJob(builder);
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
        return builder.UseModules<ModulePipeline>(includeJobs, assemblies);
    }
}
