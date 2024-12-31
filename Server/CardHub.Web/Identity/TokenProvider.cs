using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

namespace CardHub.Web.Identity;

public record ClientIdentifier(Guid ClientId, Guid SessionId);

public class TokenProvider(IConfiguration configuration)
{
    public string Create(ClientIdentifier clientIdentifier)
    {
        var secretKey = configuration["Jwt:Secret"] ?? throw new NullReferenceException();
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));

        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(
            [
                new Claim(JwtRegisteredClaimNames.Sub, clientIdentifier.ClientId.ToString()),
                new Claim("session_id", clientIdentifier.SessionId.ToString())
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