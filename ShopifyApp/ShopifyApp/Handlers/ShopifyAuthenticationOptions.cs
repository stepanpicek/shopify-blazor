using Microsoft.AspNetCore.Authentication;

namespace ShopifyApp.Handlers;

public class ShopifyAuthenticationOptions : AuthenticationSchemeOptions
{
    public const string AuthenticationScheme = "ShopifySession";
}