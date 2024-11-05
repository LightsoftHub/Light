using Light.Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace Light.Identity.SqlServer;

public class IdentityDbContextInitialiser(
    ILogger<IdentityDbContextInitialiser> logger,
    LightIdentityDbContext context,
    UserManager<User> userManager,
    RoleManager<Role> roleManager)
{
    public virtual async Task InitialiseAsync()
    {
        logger.LogInformation("Seeding...");

        try
        {
            if (context.Database.IsSqlServer() && context.Database.GetMigrations().Any())
            {
                if ((await context.Database.GetPendingMigrationsAsync()).Any())
                {
                    await context.Database.MigrateAsync();

                    var dbName = context.Database.GetDbConnection().Database;

                    logger.LogInformation("Database {name} initialized", dbName);
                }
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while initialising the database.");
            throw;
        }
    }

    public async Task TrySeedAsync()
    {
        try
        {
            if (await context.Database.CanConnectAsync())
            {
                await SeedAsync();
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while seeding the database.");
            throw;
        }
    }

    public async Task SeedAsync()
    {
        var role = new Role()
        {
            Name = "super",
            Description = "Super Admin role",
        };

        if (roleManager.Roles.All(r => r.Name != role.Name))
        {
            await roleManager.CreateAsync(role);
            logger.LogInformation("Role {name} added", role.Name);
        }

        var user = new User()
        {
            UserName = "super",
            FirstName = "Super",
            LastName = "Admin",
        };

        var defaultPassword = "123";

        if (userManager.Users.All(u => u.UserName != user.UserName))
        {
            await userManager.CreateAsync(user, defaultPassword);

            logger.LogInformation("User {name} added", user.UserName);

            await userManager.AddToRolesAsync(user, [role.Name!]);

            logger.LogInformation("Assigned role {role} to user {user}", role.Name, user.UserName);
        }

        for (var i = 1; i < 50; i++)
        {
            var normalUser = new User()
            {
                UserName = $"user{i}",
                FirstName = $"User",
                LastName = $"00{i}",
            };

            await userManager.CreateAsync(normalUser, defaultPassword);
        }

        for (var i = 1; i < 50; i++)
        {
            var tenant = new Tenant()
            {
                Name = $"Tenant_{i}",
            };

            await context.Tenants.AddAsync(tenant);
            await context.SaveChangesAsync();
        }
    }
}
