using Light.Identity.Migrator.SeedData;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Light.Identity.Migrator.MSSQL;

public class AppDbContextInitialiser(
    ILogger<AppDbContextInitialiser> logger,
    MigratorDbContext context,
    UserManager<User> userManager,
    RoleManager<Role> roleManager)
{
    public async Task InitialiseAsync()
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
            if (await context.Database.CanConnectAsync())
            {
                await SeedAsync();
            }

        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while initialising the database.");
            throw;
        }
    }

    public async Task SeedAsync()
    {
        try
        {
            await TrySeedAsync();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while seeding the database.");
            throw;
        }
    }

    public async Task TrySeedAsync()
    {
        // Default roles
        var superRole = new Role
        {
            Name = DefaultRole.NAME,
            Description = DefaultRole.DESCRIPTION
        };

        if (roleManager.Roles.All(r => r.Name != superRole.Name))
        {
            await roleManager.CreateAsync(superRole);
            logger.LogInformation("Role {name} added", superRole.Name);
        }

        // Default users
        var superUser = new User
        {
            UserName = DefaultUser.USER_NAME,
            FirstName = DefaultUser.FIRST_NAME,
            LastName = DefaultUser.LAST_NAME,
            Email = DefaultUser.EMAIL
        };

        if (userManager.Users.All(u => u.UserName != superUser.UserName))
        {
            await userManager.CreateAsync(superUser, DefaultUser.PASSWORD);
            logger.LogInformation("User {name} added", superUser.UserName);

            await userManager.AddToRolesAsync(superUser, new[] { superRole.Name });
            logger.LogInformation("Assigned role {role} to user {user}",
                superRole.Name, superUser.UserName);
        }

        // Default users
        var basicUser = new User
        {
            UserName = "user",
            FirstName = "Normal",
            LastName = "User",
            Email = "user@domain.local"
        };

        if (userManager.Users.All(u => u.UserName != basicUser.UserName))
        {
            await userManager.CreateAsync(basicUser, DefaultUser.PASSWORD);
            logger.LogInformation("User {name} added", basicUser.UserName);
        }
    }
}
