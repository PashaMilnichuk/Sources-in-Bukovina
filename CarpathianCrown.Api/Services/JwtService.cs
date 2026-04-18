using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CarpathianCrown.Api.Models;
using Microsoft.IdentityModel.Tokens;

namespace CarpathianCrown.Api.Services;

public class JwtService(IConfiguration cfg)
{
    public string Generate(User user)
    {
        var key = cfg["Jwt:Key"]!;
        var hours = int.TryParse(cfg["Jwt:Hours"], out var h) ? h : 12;

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Role, user.Role),
            new("email", user.Email)
        };

        var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
        var creds = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.UtcNow.AddHours(hours),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}