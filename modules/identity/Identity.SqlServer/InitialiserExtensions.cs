using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Light.Identity.SqlServer;

public static class InitialiserExtensions
{
    public static async Task InitialiseDatabaseAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();

        var initialiser = scope.ServiceProvider.GetRequiredService<IdentityDbContextInitialiser>();

        await initialiser.InitialiseAsync();

        await initialiser.TrySeedAsync();
    }
}