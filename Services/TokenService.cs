using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Blog.Models;
using BlogApi.Extension;
using BlogApi.JwtConfigure;
using Microsoft.IdentityModel.Tokens;

namespace BlogApi.Services
{
    public class TokenService
    {
        public string GenerateToken(User user)
        {
            var tokenHandler  = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(Configure.JwtKey);
            var claims = user.GetClaims();
            var tokenDescription = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(8),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature
                )
            };
            var token = tokenHandler.CreateToken(tokenDescription);
            return tokenHandler.WriteToken(token);
        }
    }
}