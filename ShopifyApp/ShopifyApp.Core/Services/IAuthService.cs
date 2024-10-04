using Microsoft.Extensions.Primitives;

namespace ShopifyApp.Core.Services;

public interface IAuthService
{
    Task<bool> IsValidShopifyRequestAsync(IDictionary<string,string>  query);
    Task<bool> IsShopAuthenticated(string shop);
    Task AuthenticateShopAsync(string shop, string code, string state);
    Task<string> GetAuthUrlAsync(string shop);
}