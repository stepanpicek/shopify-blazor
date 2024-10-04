using Carter;
using Microsoft.Extensions.Options;
using ShopifyApp.Components;
using ShopifyApp.Contexts;
using ShopifyApp.Core.Services;
using ShopifyApp.Entities;
using ShopifyApp.Extensions;
using ShopifyApp.Middlewares;
using ShopifyApp.Settings;
using ShopifySharp.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);
builder.Services.Configure<ShopifySettings>(builder.Configuration.GetSection("Shopify"));
builder.Services.AddSingleton(resolver =>
    resolver.GetRequiredService<IOptions<ShopifySettings>>().Value);

builder.Services.AddCarter();
// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();

builder.Services.AddPostgresDb<PostgresDbContext>(builder.Configuration.GetSection("Postgres").Get<PostgresSettings>() ?? new PostgresSettings());
builder.Services.AddDefaultIdentity<ShopifyUser>()
    .AddEntityFrameworkStores<PostgresDbContext>();

builder.Services.AddShopifySharpServiceFactories();
builder.Services.AddShopifySharpUtilities();
builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped<IAppBridgeService, AppBridgeService>();

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
