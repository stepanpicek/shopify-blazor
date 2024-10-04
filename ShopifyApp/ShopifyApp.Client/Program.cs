using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using ShopifyApp.Core.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddScoped<IAppBridgeService, AppBridgeService>();

await builder.Build().RunAsync();
