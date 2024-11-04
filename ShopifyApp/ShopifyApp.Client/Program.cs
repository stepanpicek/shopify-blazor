using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;
using ShopifyApp.Client.Handlers;
using ShopifyApp.Client.Services;
using ShopifyApp.Core.Providers;
using ShopifyApp.Core.Services;
using ShopifyApp.Core.Settings;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.Services.AddSingleton(builder.Configuration.GetSection("Shopify").Get<ShopifySettings>() ?? new ShopifySettings());
builder.Services.AddTransient<ShopifyAuthHandler>();
builder.Services.AddScoped<IAppBridgeService, AppBridgeService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IShopService, ShopService>();
builder.Services.AddScoped<INavigationWrapper, NavigationWrapper>();
builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<AuthenticationStateProvider, ShopifyAuthProvider>();
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped(sp =>
{
    var tokenService = sp.GetRequiredService<IAppBridgeService>();

    var handler = new ShopifyAuthHandler(tokenService)
    {
        InnerHandler = new HttpClientHandler()
    };
    return new HttpClient(handler) { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) };
});

builder.Services.AddMudServices();
await builder.Build().RunAsync();
