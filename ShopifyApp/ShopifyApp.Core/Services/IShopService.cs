using ShopifyApp.Core.Dto;

namespace ShopifyApp.Core.Services;

public interface IShopService
{
    Task<ShopInfoResponse> GetShopAsync(string shop);
}