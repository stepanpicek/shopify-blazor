using System.Net.Http.Json;
using ShopifyApp.Core.Dto;
using ShopifyApp.Core.Endpoints;
using ShopifyApp.Core.Services;

namespace ShopifyApp.Client.Services;

public class ShopService(HttpClient httpClient) : IShopService
{
    public async Task<ShopInfoResponse> GetShopAsync()
    {
        var response =
            await httpClient.GetFromJsonAsync<ShopInfoResponse>(
                $"{ShopifyEndpoints.ShopBase}{ShopifyEndpoints.GetShopInfo}");
        return response ?? new ShopInfoResponse();
    }
}