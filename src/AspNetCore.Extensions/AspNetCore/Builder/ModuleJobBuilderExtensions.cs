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
        // get all classes inherit from
        var modulePipelines = AsemblyTypeExtensions.GetAssignableFrom<ModuleJob>(assemblies)
            .Select(s => Activator.CreateInstance(s) as ModuleJob);

        foreach (var instance in modulePipelines)
        {
            instance?.Initialize(builder);
        }

        return builder;
    }

    /// <summary>
    /// Scan & configure module jobs pipelines by default
    /// </summary>
    public static IApplicationBuilder UseModuleJobs(this IApplicationBuilder builder, Assembly[] assemblies) =>
        builder.UseModuleJobs<ModuleJob>(assemblies);
}
