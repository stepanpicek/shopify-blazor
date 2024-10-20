namespace ShopifyApp.Core.Services;

public interface IAuthService
{
    Task<bool> IsValidShopifyRequestAsync(IDictionary<string, string> query);
    Task<bool> IsShopAuthenticatedAsync(string shop);
    Task AuthenticateShopAsync(string shop, string sessionToken);
    Task RemoveShopAsync(string shop);
    Task<string> GetShopAuthTokenAsync(string shop);
}