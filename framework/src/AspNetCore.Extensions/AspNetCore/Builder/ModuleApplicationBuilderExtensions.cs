using Light.AspNetCore.Modules;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using System.Reflection;

namespace Light.AspNetCore.Builder;

public static class ModuleApplicationBuilderExtensions
{
    /// <summary>
    /// Scan & configure all module pipelines
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="configuration"></param>
    /// <param name="assemblies"></param>
    /// <returns></returns>
    public static IApplicationBuilder AutoConfigureModulePipelines(
        this IApplicationBuilder builder,
        IConfiguration configuration,
        IEnumerable<Assembly> assemblies)
    {
        // get from all assembly if not define assemblies to scan
        assemblies ??= AppDomain.CurrentDomain.GetAssemblies();

        // get all classes inherit from interface
        var modulePipelines = assemblies
            .SelectMany(s => s.GetTypes())
            .Where(x =>
                typeof(IModuleApplicationBuilder).IsAssignableFrom(x)
                && x.IsClass && !x.IsAbstract && !x.IsGenericType)
            .Select(s => Activator.CreateInstance(s) as IModuleApplicationBuilder);

        foreach (var instance in modulePipelines)
        {
            instance?.ConfigurePipelines(builder, configuration);
        }

        return builder;
    }

    /// <summary>
    /// Scan & configure all module pipelines
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="configuration"></param>
    /// <param name="assemblies"></param>
    /// <returns></returns>
    public static IApplicationBuilder AutoConfigureModulePipelines(
        this IApplicationBuilder builder,
        IConfiguration configuration,
        params Assembly[] assemblies)
    {
        // get from all assembly if not define assemblies to scan
        if (assemblies is null || assemblies.Length == 0)
            assemblies = AppDomain.CurrentDomain.GetAssemblies();

        builder.AutoConfigureModulePipelines(configuration, assemblies.AsEnumerable());

        return builder;
    }
}
