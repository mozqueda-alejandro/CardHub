using System.Security.Claims;
using System.Text;
using CardHub.Application.Games.Shared;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

namespace CardHub.Application.Identity;

public record ClientIdentifier(Guid ClientId, Guid SessionId);

public class TokenProvider(IConfiguration configuration)
{
    public string Create(IClient clientIdentifier)
    {
        var secret = configuration["Jwt:Secret"] ?? throw new NullReferenceException();
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(
            [
                new Claim(JwtRegisteredClaimNames.Sub, clientIdentifier.ClientId.ToString()),
                new Claim("RoomId", clientIdentifier.ClientId.ToString()),
                new Claim("Role", "gameboard")
            ]),
            Audience = configuration["Jwt:Audience"],
            Issuer = configuration["Jwt:Issuer"],
            Expires = DateTime.UtcNow.AddMinutes(configuration.GetValue<int>("Jwt:ExpirationInMinutes")),
            SigningCredentials = credentials,
        };
        
        var handler = new JsonWebTokenHandler();
        var token = handler.CreateToken(tokenDescriptor);
        return token;
    }
}