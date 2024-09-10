global using Light.AspNetCore.Mvc;
global using Light.Repositories;
using Light.AspNetCore.Builder;
using Light.AspNetCore.Swagger;
using Light.Caching.Infrastructure;
using Light.Extensions.DependencyInjection;
using Light.Serilog;
using Light.SmtpMail;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Sample.Data;
using Sample.HealthChecks;
using Sample.SoapCore;
using Sample.TestOption;
using Serilog;
using System.Reflection;

SerilogExtensions.EnsureInitialized();

try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.LoadJsonConfigurations(["configurations"]);
    //builder.AddConfigurations(["D:", "configurations"]);
    //builder.AddJsonFiles();
    builder.Host.ConfigureSerilog();

    // Add services to the container.

    /*
    builder.Services.AddGraphMail(opt =>
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

    builder.Services.Configure<SmtpSettings>(builder.Configuration.GetSection("SMTPMail"));

    builder.Services.AddFileGenerator();

    builder.Services.AddTestOptions(builder.Configuration);

    // Overide by BindConfiguration
    var issuer = builder.Configuration.GetValue<string>("JWT:Issuer");
    var key = builder.Configuration.GetValue<string>("JWT:SecretKey");
    builder.Services.AddJwtAuth(issuer!, key!);

    //builder.Services.AddTelegram();

    builder.Services
        .AddControllers()
        .AddInvalidModelStateHandler();

    builder.Services.AddApiVersion(1);
    builder.Services.AddSwagger(builder.Configuration, true);
    builder.Services.TryAddEnumerable(
        ServiceDescriptor.Transient<IApiDescriptionProvider, SubgroupDescriptionProvider>());

    //builder.Services.AddGlobalExceptionHandler();

    builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(AppDomain.CurrentDomain.GetAssemblies()));
    builder.Services.AutoAddDependencies();

    builder.Services.ScanModuleServices(builder.Configuration, executingAssembly);

    builder.Services.AddAppSoapCore();

    builder.Services.AddHealthChecksService();

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger(builder.Configuration, true);
    }

    app.UseGuidTraceId();
    //app.UseMiddlewares(builder.Configuration);
    app.UseLightRequestLogging(builder.Configuration);
    app.UseLightExceptionHandler(); // must inject after Inbound Logging

    //app.UseExceptionHandler();

    app.UseHttpsRedirection();

    app.UseAuthentication();
    app.UseAuthorization();

    app.UseAppSoapCore();

    app.ScanModulePipelines(builder.Configuration, executingAssembly);
    app.ScanModuleJobs(builder.Configuration, executingAssembly);

    app.MapControllers();

    app.MapHealthChecksEndpoint();

    app.Run();
}
catch (Exception ex) when (!ex.GetType().Name.Equals("StopTheHostException", StringComparison.Ordinal))
{
    SerilogExtensions.EnsureInitialized();
    Log.Fatal(ex, "Unhandled exception");
}
finally
{
    Log.Information("Shut down complete.");
    Log.CloseAndFlush();
}