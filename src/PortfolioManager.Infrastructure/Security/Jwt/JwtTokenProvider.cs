using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

using PortfolioManager.Infrastructure.Common;

namespace PortfolioManager.Infrastructure.Security.Jwt;

public interface IJwtTokenProvider
{
    public string GenerateToken(string userid, string firstName, string email, bool isAdmin);
}

public class JwtTokenProvider : IJwtTokenProvider
{
    private readonly TimeSpan _tokenLifetime;
    private readonly JwtSettings _options;
    private readonly IDateTimeProvider _dateTimeProvider;

    public JwtTokenProvider(IOptions<JwtSettings> options, IDateTimeProvider dateTimeProvider)
    {
        ArgumentNullException.ThrowIfNull(options);
        ArgumentNullException.ThrowIfNull(dateTimeProvider);

        _options = options.Value;
        _tokenLifetime = TimeSpan.FromMinutes(_options.ExpiryMinutes);
        _dateTimeProvider = dateTimeProvider;
    }

    public string GenerateToken(string userid, string firstName, string email, bool isAdmin)
    {
        var keyBytes = Encoding.UTF8.GetBytes(_options.Secret);
        List<Claim> claims =
        [
            new(JwtRegisteredClaimNames.Sub, userid),
            new(JwtRegisteredClaimNames.GivenName, firstName),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new(JwtRegisteredClaimNames.Email, email),
            new("userid", userid),
            new("admin", isAdmin.ToString().ToLower())
        ];
        var credentials = new SigningCredentials(new SymmetricSecurityKey(keyBytes), SecurityAlgorithms.HmacSha256);
        var tokenDescriptor = new SecurityTokenDescriptor()
        {
            Subject = new ClaimsIdentity(claims),
            Expires = _dateTimeProvider.UtcNow.Add(_tokenLifetime),
            NotBefore = _dateTimeProvider.UtcNow,
            IssuedAt = _dateTimeProvider.UtcNow,
            Issuer = _options.Issuer,
            Audience = _options.Audience,
            SigningCredentials = credentials,
        };
        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }
}