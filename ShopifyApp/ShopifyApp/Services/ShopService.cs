using ShopifyApp.Core.Dto;
using ShopifyApp.Core.Services;

namespace ShopifyApp.Services;

public class ShopService : IShopService
{
    private readonly IAuthService _authService;

    public ShopService(IAuthService authService)
    {
        _authService = authService;
    }

    public async Task<ShopInfoResponse> GetShopAsync(string shop)
    {
        var accessToken = await _authService.GetShopAuthTokenAsync(shop);
        var service = new ShopifySharp.ShopService($"https://{shop}", accessToken);

        var shopInfo = await service.GetAsync();
        return new ShopInfoResponse
        {
            Email = shopInfo.Email,
            ShopName = shopInfo.Name,
            Owner = shopInfo.ShopOwner
        };
    }
}