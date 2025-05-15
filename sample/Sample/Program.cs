global using Light.AspNetCore.Mvc;
global using Light.Repositories;
using Light.ActiveDirectory;
using Light.AspNetCore.Builder;
using Light.AspNetCore.Middlewares;
using Light.AspNetCore.Swagger;
using Light.Caching.Infrastructure;
using Light.Extensions.DependencyInjection;
using Light.Serilog;
using Light.SmtpMail;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Sample.Data;
using Sample.HealthChecks;
using Sample.Identity;
using Sample.SoapCore;
using Sample.TestOption;
using Serilog;
using System.Reflection;

Serilogger.EnsureInitialized();
Log.Information("Application start...");

try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.LoadConfigurationFrom(["configurations"]);
    //builder.AddConfigurations(["D:", "configurations"]);
    //builder.AddJsonFiles();
    builder.Host.ConfigureSerilog();

    // Add services to the container.
    /*
    builder.Services.AddMicrosoftGraph(opt =>
    {
        opt.ClientSecret = "";
        opt.ClientId = "";
        opt.TenantId = "";
    });
    */
    var executingAssembly = Assembly.GetExecutingAssembly();

    builder.Services.AddData(builder.Configuration);
    var settings = builder.Configuration.GetSection("Caching").Get<CacheOptions>();
    builder.Services.AddCache(opt =>
    {
        opt.Provider = settings!.Provider;
        opt.RedisHost = settings.RedisHost;
        opt.RedisPassword = settings.RedisPassword;
    });

    builder.Services.AddFileGenerator();

    builder.Services.AddTestOptions(builder.Configuration);

    // Overide by BindConfiguration
    var issuer = builder.Configuration.GetValue<string>("JWT:Issuer");
    var key = builder.Configuration.GetValue<string>("JWT:SecretKey");
    builder.Services.AddJwtAuth(issuer!, key!);
    builder.Services.AddActiveDirectory(opt => opt.Name = "domain.local");

    //builder.Services.AddTelegram();

    builder.Services.AddOptions<RequestLoggingOptions>().BindConfiguration("RequestLogging");

    builder.Services
        .AddControllers()
        .AddInvalidModelStateHandler()
        .AddDefaultJsonOptions();

    builder.Services.AddApiVersion(1);
    builder.Services.AddSwagger(builder.Configuration);

    //builder.Services.AddGlobalExceptionHandler();

    builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(AppDomain.CurrentDomain.GetAssemblies()));
    builder.Services.AutoAddDependencies();

    builder.Services.AddInfrastructureIdentity(builder.Configuration);

    builder.Services.AddModules(builder.Configuration, [executingAssembly]);

    builder.Services.AddAppSoapCore();

    builder.Services.AddHealthChecksService();

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    app.UseSwagger();

    app.UseUlidTraceId();
    //app.UseMiddlewares(builder.Configuration);
    app.UseLightRequestLogging();
    app.UseLightExceptionHandler(); // must inject after Inbound Logging

    //app.UseExceptionHandler();

    app.UseHttpsRedirection();

    app.UseAuthentication();
    app.UseAuthorization();

    app.UseAppSoapCore();

    app.UseModules([executingAssembly]);

    app.MapControllers();

    app.MapHealthChecksEndpoint();

    app.SeedData();

    app.Run();
}
catch (Exception ex) when (!ex.GetType().Name.Equals("StopTheHostException", StringComparison.Ordinal))
{
    Serilogger.EnsureInitialized();
    Log.Fatal(ex, "Unhandled exception");
}
finally
{
    Log.Information("Shut down complete.");
    Log.CloseAndFlush();
}