using Carter;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Options;
using MudBlazor.Services;
using ShopifyApp.Components;
using ShopifyApp.Contexts;
using ShopifyApp.Core.Providers;
using ShopifyApp.Core.Services;
using ShopifyApp.Core.Settings;
using ShopifyApp.Entities;
using ShopifyApp.Extensions;
using ShopifyApp.Handlers;
using ShopifyApp.Middlewares;
using ShopifyApp.Repositories;
using ShopifyApp.Services;
using ShopifyApp.Workers;
using ShopifySharp.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

//Workers
builder.Services.AddHostedService<ActionQueueWorker>();

//Config
builder.Services.Configure<ShopifySettings>(builder.Configuration.GetSection("Shopify"));
builder.Services.AddSingleton(resolver =>
    resolver.GetRequiredService<IOptions<ShopifySettings>>().Value);

builder.Services.AddCarter();

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();

//DB
builder.Services.AddMySqlDb<AppDbContext>(builder.Configuration.GetSection("MySQL").Get<MysqlSettings>() ?? new MysqlSettings());
builder.Services.AddDefaultIdentity<ShopifyUser>()
    .AddEntityFrameworkStores<AppDbContext>();

builder.Services.AddCascadingAuthenticationState();
builder.Services.AddShopifySharpServiceFactories();
builder.Services.AddShopifySharpUtilities();
builder.Services.AddHttpContextAccessor();
builder.Services.AddHttpClient();

builder.Services.AddScoped<IAppBridgeService, AppBridgeService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IShopService, ShopService>();
builder.Services.AddScoped<AuthenticationStateProvider, ShopifyAuthProvider>();
builder.Services.AddScoped<INavigationWrapper, NavigationWrapper>();
builder.Services.AddScoped<IActionRepository, ActionRepository>();
builder.Services.AddScoped<ITriggerRepository, TriggerRepository>();
builder.Services.AddScoped<IWebhookRepository, WebhookRepository>();
builder.Services.AddMudServices();

builder.Services.AddAuthentication()
    .AddScheme<ShopifyAuthenticationOptions, ShopifyAuthenticationHandler>(
        ShopifyAuthenticationOptions.AuthenticationScheme, 
        _ => {});

var app = builder.Build();

app.UseMiddleware<SecureResponseMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();
app.UseStaticFiles();
app.UseAntiforgery();
app.MapCarter();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(ShopifyApp.Client._Imports).Assembly);

app.Run();
