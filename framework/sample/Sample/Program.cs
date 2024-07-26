global using Light.Repositories;
using Light.AspNetCore.Hosting;
using Light.AspNetCore.Hosting.ExceptionHandler;
using Light.AspNetCore.Hosting.JwtAuth;
using Light.AspNetCore.Hosting.Middlewares;
using Light.AspNetCore.Hosting.Swagger;
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



builder.Services.AddData(builder.Configuration);
var settings = builder.Configuration.GetSection("Caching").Get<CacheOptions>();
builder.Services.AddCache(opt =>
{
    opt.Provider = settings!.Provider;
    opt.RedisHost = settings.RedisHost;
    opt.RedisPassword = settings.RedisPassword;
});

builder.Services.Configure<SmtpSettings>(builder.Configuration.GetSection("SMTPMail"));

builder.Services.AddFiles();

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

builder.Services.AutoConfigureModuleServices(builder.Configuration);

builder.Services.AddAppSoapCore();

builder.Services.AddHealthChecksService();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger(builder.Configuration, true);
}

app.UseMiddleware<TraceIdMiddleware>();
//app.UseMiddlewares(builder.Configuration);
app.UseRequestLoggingMiddleware(builder.Configuration);
app.UseExceptionHandlerMiddleware(); // must inject after Inbound Logging

//app.UseExceptionHandler();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.UseAppSoapCore();

app.AutoConfigureModulePipelines(builder.Configuration);

app.MapControllers();

app.MapHealthChecksEndpoint();

app.Run();
