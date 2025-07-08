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

        services.AddTransient<MediatorImp>();

        services.AddTransient<IMediator>(sp => sp.GetRequiredService<MediatorImp>());
        services.AddTransient<ISender>(sp => sp.GetRequiredService<MediatorImp>());
        services.AddTransient<IPublisher>(sp => sp.GetRequiredService<MediatorImp>());

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
            services.AddTransient(handler.Interface, handler.Implementation);
        }
    }

    public static IServiceCollection AddBehaviors(this IServiceCollection services, params Type[] behaviorTypes)
    {
        foreach (var behaviorType in behaviorTypes)
        {
            var pipelineType = typeof(IPipelineBehavior<,>);

            var descriptor = new ServiceDescriptor(
                pipelineType,
                behaviorType,
                ServiceLifetime.Transient);

            services.Add(descriptor);
        }

        return services;
    }
}
