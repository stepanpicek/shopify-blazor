using System.Net.Http.Headers;
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
        HttpRequest request,
        [FromBody] string shop,
        [FromServices] IAuthService authService)
    {
        AuthenticationHeaderValue.TryParse(request.Headers.Authorization, out var sessionToken);
        await authService.AuthenticateShopAsync(shop, sessionToken?.Parameter ?? string.Empty);
        return Results.Ok();
    }
}