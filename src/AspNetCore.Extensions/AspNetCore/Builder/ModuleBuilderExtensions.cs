﻿using Light.AspNetCore.Modularity;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using System.Reflection;

namespace Light.AspNetCore.Builder;

public static class ModuleBuilderExtensions
{
    /// <summary>
    /// Scan & configure module pipelines
    /// </summary>
    public static IApplicationBuilder UseModules<T>(this IApplicationBuilder builder, Assembly[] assemblies)
        where T : IModuleBuilder
    {
        // get all classes inherit from interface
        var modulePipelines = AsemblyTypeExtensions.GetAssignableFrom<IModuleBuilder>(assemblies)
            .Select(s => Activator.CreateInstance(s) as IModuleBuilder);

        foreach (var instance in modulePipelines)
        {
            instance?.Configure(builder);
        }

        return builder;
    }

    /// <summary>
    /// Scan & configure module pipelines by default
    /// </summary>
    /// <returns></returns>
    public static IApplicationBuilder UseModules(this IApplicationBuilder builder, Assembly[] assemblies) =>
        builder.UseModules<ModulePipeline>(assemblies);

    /// <summary>
    /// Scan & map module endpoints
    /// </summary>
    public static IEndpointRouteBuilder MapHubs<T>(this IEndpointRouteBuilder builder, Assembly[] assemblies)
        where T : IModuleBuilder
    {
        // get all classes inherit from interface
        var modulePipelines = AsemblyTypeExtensions.GetAssignableFrom<IModuleBuilder>(assemblies)
            .Select(s => Activator.CreateInstance(s) as IModuleBuilder);

        foreach (var instance in modulePipelines)
        {
            instance?.MapHub(builder);
        }

        return builder;
    }

    /// <summary>
    /// Scan & map module endpoints by default
    /// </summary>
    public static IEndpointRouteBuilder MapHubs(this IEndpointRouteBuilder builder, Assembly[] assemblies) =>
        builder.MapHubs<ModulePipeline>(assemblies);

    /// <summary>
    /// Scan & map module endpoints
    /// </summary>
    public static IEndpointRouteBuilder MapModuleEndpoints<T>(this IEndpointRouteBuilder builder, Assembly[] assemblies)
        where T : IModuleBuilder
    {
        // get all classes inherit from interface
        var modulePipelines = AsemblyTypeExtensions.GetAssignableFrom<IModuleBuilder>(assemblies)
            .Select(s => Activator.CreateInstance(s) as IModuleBuilder);

        foreach (var instance in modulePipelines)
        {
            instance?.MapEndpoints(builder);
        }

        return builder;
    }

    /// <summary>
    /// Scan & map module endpoints by default
    /// </summary>
    public static IEndpointRouteBuilder MapModuleEndpoints(this IEndpointRouteBuilder builder, Assembly[] assemblies) =>
        builder.MapModuleEndpoints<ModulePipeline>(assemblies);

    /// <summary>
    /// Scan & configure module pipelines
    /// </summary>
    public static IApplicationBuilder UseModules<T>(this WebApplication app, Assembly[] assemblies)
        where T : IModuleBuilder
    {
        // get all classes inherit from interface
        var modulePipelines = AsemblyTypeExtensions.GetAssignableFrom<IModuleBuilder>(assemblies)
            .Select(s => Activator.CreateInstance(s) as IModuleBuilder);

        foreach (var instance in modulePipelines)
        {
            instance?.Configure(app);
        }

        return app;
    }

    /// <summary>
    /// Scan & configure module pipelines by default
    /// </summary>
    /// <returns></returns>
    public static IApplicationBuilder UseModules(this WebApplication app, Assembly[] assemblies) =>
        app.UseModules<ModulePipeline>(assemblies);
}
