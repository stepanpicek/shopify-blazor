using Carter;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ShopifyApp.Core.Services;
using ShopifyApp.Entities;
using ShopifyApp.Filters;

namespace ShopifyApp.Endpoints;

public class ShopifyEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/shopify")
            .AddEndpointFilter<ShopifyRequestFilter>();
        group.MapGet("/install", Install);
        group.MapGet("/callback", Callback);
        group.MapGet("/is-authenticated", IsAuthenticated);
    }

    private static async Task<IResult> Install(
        [FromQuery] string shop,
        [FromServices] IAuthService authService)
    {
        var authorizationUrl = await authService.GetAuthUrlAsync(shop);
        return Results.Redirect(authorizationUrl);
    }

    private static async Task<IResult> Callback(
        [FromQuery] string shop,
        [FromQuery] string host,
        [FromQuery] string state,
        [FromQuery] string code,
        [FromServices] IAuthService authService)
    {
        try
        {
            await authService.AuthenticateShopAsync(shop, code, state);
            return Results.Redirect("/shopify?host=" + host);
        }
        catch (Exception)
        {
            return Results.Forbid();
        }
    }

    private static async Task<IResult> IsAuthenticated(string code, [FromServices] UserManager<ShopifyUser> userManager)
    {
        return Results.Forbid();
    }
}