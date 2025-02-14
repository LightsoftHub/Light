using Light.ActiveDirectory.Interfaces;
using Light.ActiveDirectory.Options;
using Light.ActiveDirectory.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Light.ActiveDirectory;

public static class ActiveDirectoryModule
{
    public static IServiceCollection AddActiveDirectory(this IServiceCollection services)
    {
        services.AddTransient<IActiveDirectoryService, FakeActiveDirectoryService>();

        return services;
    }

    public static IServiceCollection AddActiveDirectory(this IServiceCollection services, Action<DomainOptions> action)
    {
        services.Configure(action);

#pragma warning disable CA1416
        services.AddTransient<IActiveDirectoryService, ActiveDirectoryService>();
#pragma warning restore CA1416

        return services;
    }

    public static IServiceCollection AddLdapActiveDirectory(this IServiceCollection services, Action<LdapOptions> action)
    {
        services.Configure(action);

#pragma warning disable CA1416
        services.AddTransient<IActiveDirectoryService, LDAPService>();
#pragma warning restore CA1416

        return services;
    }
}