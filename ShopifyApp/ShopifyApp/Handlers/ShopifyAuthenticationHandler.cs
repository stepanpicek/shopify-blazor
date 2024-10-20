using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using ShopifyApp.Core.Dto;
using ShopifyApp.Core.Settings;
using ShopifyApp.Entities;

namespace ShopifyApp.Handlers;

public class ShopifyAuthenticationHandler(
    IOptionsMonitor<ShopifyAuthenticationOptions> options,
    ILoggerFactory logger,
    UrlEncoder encoder)
    : AuthenticationHandler<ShopifyAuthenticationOptions>(options, logger, encoder)
{
    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        if (!AuthenticationHeaderValue.TryParse(Context.Request.Headers["Authorization"], out var authHeader)
            || !authHeader.Scheme.Equals("bearer", StringComparison.OrdinalIgnoreCase))
        {
            return AuthenticateResult.Fail("Missing or invalid Authorization header");
        }

        var settings = Context.RequestServices.GetRequiredService<ShopifySettings>();
        var userManager = Context.RequestServices.GetRequiredService<UserManager<ShopifyUser>>();
        var handler = new JsonWebTokenHandler();
        var validationParams = new TokenValidationParameters
        {
            IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(settings.ClientSecret)),
            ValidateIssuerSigningKey = true,
            ValidateActor = false,
            ValidateIssuer = false,
            ValidateLifetime = true,
            ValidAudience = settings.ClientId,
            ValidateAudience = true
        };
        
        var validationResult = await handler.ValidateTokenAsync(authHeader.Parameter, validationParams);
        if (!validationResult.IsValid)
        {
            return AuthenticateResult.Fail("Authorization header failed validation.");
        }
        
        var token = handler.ReadJsonWebToken(authHeader.Parameter);
        var shopDomain = token.GetPayloadValue<string>("dest");
        var _logger = logger.CreateLogger<ShopifyAuthenticationHandler>();
        var uri = new Uri(shopDomain);
        _logger.LogInformation($"Shopify domain: {uri.Host}");
        if (await userManager.FindByNameAsync(uri.Host) is null)
        {
            return AuthenticateResult.Fail("Shop not found");
        }
        
        var session = new SessionToken()
        {
            Id = token.Id,
            StoreUserId = token.Subject,
            SessionId = token.GetPayloadValue<string>("sid"),
            Issuer = token.Issuer,
            ShopDomainWithProtocol = shopDomain,
            ShopDomain = uri.Host,
            ApiKey = token.Audiences.FirstOrDefault(),
            IssuedAt = token.IssuedAt,
            ValidFrom = token.ValidFrom,
            ValidTo = token.ValidTo,
        };

        var principal = new ClaimsPrincipal(session);
        var ticket = new AuthenticationTicket(principal, Scheme.Name);

        return AuthenticateResult.Success(ticket);
    }
}