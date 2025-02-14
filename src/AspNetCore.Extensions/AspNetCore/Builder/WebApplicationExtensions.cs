using Light.AspNetCore.Modularity;
using Microsoft.AspNetCore.Builder;
using System.Reflection;

namespace Light.AspNetCore.Builder;

public static class WebApplicationExtensions
{
    /// <summary>
    /// Scan & configure module pipelines
    /// </summary>
    public static WebApplication UseModules<T>(this WebApplication builder, Assembly[] assemblies)
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
            .Select(s => Activator.CreateInstance(s) as IModuleBuilder);

        foreach (var instance in modulePipelines)
        {
            instance?.ConfigurePipelines(builder);
            instance?.MapEndpoints(builder);
        }

        return builder;
    }

    /// <summary>
    /// Scan & configure module pipelines
    /// </summary>
    public static WebApplication UseModules(this WebApplication builder, Assembly[] assemblies)
    {
        return builder.UseModules<ModulePipeline>(assemblies);
    }
}
