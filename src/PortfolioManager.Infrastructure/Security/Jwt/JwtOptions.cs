namespace PortfolioManager.Infrastructure.Security.Jwt;

public class JwtOptions
{
    public required string Key { get; init; }
    public required string Issuer { get; init; }
    public required string Audience { get; init; }
}