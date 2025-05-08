global using Light.Contracts;
global using Light.Extensions;
using BlazorSample.Client.Services;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddSingleton<ProductService>();

await builder.Build().RunAsync();
