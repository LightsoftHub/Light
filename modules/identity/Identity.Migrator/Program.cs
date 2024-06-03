global using Light.Identity.EntityFrameworkCore;
global using Light.Identity.EntityFrameworkCore.Models;
global using Light.Identity.Migrator.Data;
using Light.Identity.Migrator.Extensions;
using Light.Identity.Migrator.MSSQL;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

// set Environment
//Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Live");

using var host = Host.CreateHostBuilder(args).Build();
using var scope = host.Services.CreateScope();
var serviceProvider = scope.ServiceProvider;

var initialiser = serviceProvider.GetRequiredService<AppDbContextInitialiser>();
await initialiser.InitialiseAsync();
await initialiser.SeedAsync();

var _logger = serviceProvider.GetRequiredService<ILogger<AppDbContextInitialiser>>();
_logger.LogInformation("Done.");
Console.ReadKey();
