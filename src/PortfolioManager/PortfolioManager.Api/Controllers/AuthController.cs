using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

using PortfolioManager.Api.Models;

namespace PortfolioManager.Api.Controllers;

public class AuthController(IConfiguration configuration, ApplicationDbContext context) : ControllerBase
{
    private static readonly TimeSpan TokenLifetime = TimeSpan.FromHours(1);

    private readonly IConfiguration _configuration = configuration;
    private readonly ApplicationDbContext _context = context;

    [HttpPost]
    [Route("register")]
    public async Task<IActionResult> Register([FromBody] UserRegistrationModel model, CancellationToken cancellationToken)
    {
        var user = await _context.User.FirstOrDefaultAsync(u => u.Username == model.Username, cancellationToken);

        if (user is not null)
            return BadRequest("User already exists");
    
        user = new()
        {
            Username = model.Username,
            PasswordHash = HashPassword(model.Password),
            IsAdmin = model.IsAdmin
        };

        _context.User.Add(user);

        await _context.SaveChangesAsync(cancellationToken);

        return Ok(new { user.Id, user.Username });
    }

    [HttpPost("token")]
    public async Task<IActionResult> GenerateToken([FromBody] TokenGenerationRequest request, CancellationToken cancellationToken)
    {
        var user = await _context.User.FirstOrDefaultAsync(u => u.Username == request.Username, cancellationToken);

        if (user == null || !VerifyPassword(request.Password, user.PasswordHash))
            return Unauthorized("Invalid username or password.");

        JwtSecurityTokenHandler tokenHandler = new();
        byte[] keyBytes = Encoding.UTF8.GetBytes(_configuration["JwtSettings:Key"]!);
        List<Claim> claims =
        [
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new(JwtRegisteredClaimNames.Sub, user.Username),
            new(JwtRegisteredClaimNames.Email, user.Username),
            new("userid", user.Id.ToString()),
            new("admin", user.IsAdmin.ToString().ToLower())
        ];

        //foreach (var claimPair in request.CustomClaims)
        //{
        //    var jsonElement = claimPair.Value;
        //    var valueType = jsonElement.ValueKind switch
        //    {
        //        JsonValueKind.True => ClaimValueTypes.Boolean,
        //        JsonValueKind.False => ClaimValueTypes.Boolean,
        //        JsonValueKind.Number => ClaimValueTypes.Double,
        //        _ => ClaimValueTypes.String
        //    };

        //    var claim = new Claim(claimPair.Key, claimPair.Value.ToString()!, valueType);
        //    claims.Add(claim);
        //}

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.Add(TokenLifetime),
            NotBefore = DateTime.UtcNow,
            IssuedAt = DateTime.UtcNow,
            Issuer = _configuration["JwtSettings:Issuer"]!,
            Audience = _configuration["JwtSettings:Audience"]!,
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(keyBytes), SecurityAlgorithms.HmacSha256),
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        var jwt = tokenHandler.WriteToken(token);

        return Ok(jwt);
    }

    private static bool VerifyPassword(string password, string storedHash)
    {
        return BCrypt.Net.BCrypt.Verify(password, storedHash);
    }

    private static string HashPassword(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }
}