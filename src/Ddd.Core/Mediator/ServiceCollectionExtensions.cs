using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Light.Mediator;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddMediatorFromAssemblies(this IServiceCollection services, params Assembly[] assemblies)
    {
        if (assemblies is null || assemblies.Length == 0)
        {
            throw new Exception("At least one assembly must be provided.");
        }

        services.AddScoped<MediatorImp>();
        services.AddScoped<IMediator>(sp => sp.GetRequiredService<MediatorImp>());
        services.AddScoped<ISender>(sp => sp.GetRequiredService<MediatorImp>());
        services.AddScoped<IPublisher>(sp => sp.GetRequiredService<MediatorImp>());

        services.AddHandlers(typeof(IRequestHandler<,>), assemblies);
        services.AddHandlers(typeof(INotificationHandler<>), assemblies);

        return services;
    }

    private static void AddHandlers(this IServiceCollection services, Type handlerInterfaceType, Assembly[] assemblies)
    {
        var handlerTypes = assemblies
            .SelectMany(s => s.GetTypes())
            .Where(type => !type.IsAbstract && !type.IsInterface)
            .SelectMany(type => type.GetInterfaces()
                .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == handlerInterfaceType)
                .Select(i => new { Interface = i, Implementation = type }));

        foreach (var handler in handlerTypes)
        {
            services.AddScoped(handler.Interface, handler.Implementation);
        }
    }

    public static IServiceCollection AddPipelinesFromAssemblies(this IServiceCollection services, params Assembly[] assemblies)
    {
        if (assemblies is null || assemblies.Length == 0)
        {
            throw new Exception("At least one assembly must be provided.");
        }

        var pipelineInterfaceType = typeof(IPipelineBehavior<,>);

        var handlerTypes = assemblies
            .SelectMany(s => s.GetTypes())
            .Where(type => !type.IsAbstract && !type.IsInterface)
            .SelectMany(type => type.GetInterfaces()
                .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == pipelineInterfaceType)
                .Select(i => new { Implementation = type }));

        foreach (var handler in handlerTypes)
        {
            services.AddTransient(pipelineInterfaceType, handler.Implementation);
        }

        return services;
    }
}
