using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace PortfolioManager.Infrastructure.Security.Jwt;

public interface IJwtProvider
{
    string Generate(string userid, string username, bool isAdmin);
}

public class JwtProvider(IOptions<JwtOptions> options) : IJwtProvider
{
    private static readonly TimeSpan TokenLifetime = TimeSpan.FromMinutes(30);

    private readonly JwtOptions _options = options.Value;

    public string Generate(string userid, string username, bool isAdmin)
    {
        byte[] keyBytes = Encoding.UTF8.GetBytes(_options.Key);
        List<Claim> claims =
        [
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new(JwtRegisteredClaimNames.Sub, username),
            new(JwtRegisteredClaimNames.Email, username),
            new("userid", userid),
            new("admin", isAdmin.ToString().ToLower())
        ];
        SigningCredentials credentials = new(new SymmetricSecurityKey(keyBytes), SecurityAlgorithms.HmacSha256);
        SecurityTokenDescriptor tokenDescriptor = new()
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.Add(TokenLifetime),
            NotBefore = DateTime.UtcNow,
            IssuedAt = DateTime.UtcNow,
            Issuer = _options.Issuer,
            Audience = _options.Audience,
            SigningCredentials = credentials,
        };
        JwtSecurityTokenHandler tokenHandler = new();
        SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }
}