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

        services.AddScoped<ISender, Sender>();

        var handlerInterfaceType = typeof(IRequestHandler<,>);

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

        return services;
    }
}
