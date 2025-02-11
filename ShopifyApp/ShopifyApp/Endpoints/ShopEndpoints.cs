using Carter;
using Microsoft.AspNetCore.Authorization;
using ShopifyApp.Core.Dto;
using ShopifyApp.Core.Services;
using ShopifyApp.Handlers;
using ShopifySharp;

namespace ShopifyApp.Endpoints;

public class ShopEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup(Core.Endpoints.ShopifyEndpoints.ShopBase);
        group.MapGet(Core.Endpoints.ShopifyEndpoints.GetShopInfo,GetShopInfoAsync);
    }

    [Authorize(AuthenticationSchemes = ShopifyAuthenticationOptions.AuthenticationScheme)]
    private async Task<IResult> GetShopInfoAsync(
        HttpContext context,
        IAuthService authService)
    {
        var identity = context.User.Identity as SessionToken;
        var accessToken = await authService.GetShopAuthTokenAsync(identity.ShopDomain);
        var service = new ShopService(identity.ShopDomainWithProtocol, accessToken);

        var shop = await service.GetAsync();
        return Results.Ok(new ShopInfoResponse
        {
            Email = shop.Email,
            ShopName = shop.Name,
            Owner = shop.ShopOwner
        });
    }
}