using AuthLibrary.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AuthLibrary
{
    internal static class TokenService
    {
        internal static string GenerateJwtToken(string email, string role, TokenSettings tokenSettings)
        {
            var secretKey = tokenSettings.SecretKey ?? throw new ArgumentException("Secret key not found");
            var key = Encoding.ASCII.GetBytes(secretKey);

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(
                [
                    new Claim(ClaimTypes.Email, email),
                    new Claim(ClaimTypes.Role, role)
                ]),
                Issuer = tokenSettings.Issuer,
                Audience = tokenSettings.Audience,
                Expires = DateTime.UtcNow.AddMinutes(tokenSettings.ExpirationMinutes),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);
            return tokenString;
        }
    }
}
