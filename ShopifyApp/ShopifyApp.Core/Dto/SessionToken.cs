using System.Security.Claims;

namespace ShopifyApp.Core.Dto;

public class SessionToken : ClaimsIdentity
{
    public string? Id { get; set; }

    public string? StoreUserId { get; set; }

    public string? SessionId { get; set; }

    public string? Issuer { get; set; }
        
    public string? ShopDomain { get; set; }

    public string? ShopDomainWithProtocol { get; set; }

    public string? ApiKey { get; set; }

    public DateTime IssuedAt { get; set; }

    public DateTime ValidFrom { get; set; }

    public DateTime ValidTo { get; set; }

    public override bool IsAuthenticated { get; } = true;
}