using Carter;
using Microsoft.AspNetCore.Mvc;
using ShopifyApp.Core.Services;

namespace ShopifyApp.Endpoints;

public class ShopifyEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup(Core.Endpoints.ShopifyEndpoints.ApiBase);
        group.MapPost(Core.Endpoints.ShopifyEndpoints.IsShopAuthenticated, IsShopAuthenticatedAsync);
        group.MapPost(Core.Endpoints.ShopifyEndpoints.IsValidShopifyRequest, IsValidShopifyRequestAsync);
        group.MapPost(Core.Endpoints.ShopifyEndpoints.Authentication, AuthenticationAsync);
    }

    private static async Task<IResult> IsShopAuthenticatedAsync(
        [FromBody] string shop,
        [FromServices] IAuthService authService)
    {
        return Results.Ok(await authService.IsShopAuthenticatedAsync(shop));
    }

    private static async Task<IResult> IsValidShopifyRequestAsync(
        [FromBody] IDictionary<string, string> query, 
        [FromServices] IAuthService authService)
    {
        return Results.Ok(await authService.IsValidShopifyRequestAsync(query));
    }

    private static async Task<IResult> AuthenticationAsync(
        [FromBody] string shop,
        [FromHeader(Name = "Authorization")] string sessionToken,
        [FromServices] IAuthService authService)
    {
        await authService.AuthenticateShopAsync(shop, sessionToken);
        return Results.Ok();
    }
}