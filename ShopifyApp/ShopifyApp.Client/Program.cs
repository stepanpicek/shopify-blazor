using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using ShopifyApp.Client.Services;
using ShopifyApp.Core.Services;
using ShopifyApp.Core.Settings;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.Services.AddSingleton(builder.Configuration.GetSection("Shopify").Get<ShopifySettings>() ?? new ShopifySettings());
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddScoped<IAppBridgeService, AppBridgeService>();
builder.Services.AddScoped<IAuthService, AuthService>();

await builder.Build().RunAsync();
