using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using WEB_API.BL.API;

namespace WEB_API.BL.Services
{
    public class JwtService : IJwtService
    {
        private readonly string _issuer;
        private readonly string _audience;
        private readonly string _secretKey;

        public JwtService(IConfiguration configuration)
        {
            _issuer = configuration["JwtSettings:Issuer"];
            _audience = configuration["JwtSettings:Audience"];
            _secretKey = configuration["JwtSettings:Key"];
        }

        public static string GenerateSecretKey(int length = 32)
        {
            using (var rng = RandomNumberGenerator.Create())
            {
                var secretKey = new byte[length];
                rng.GetBytes(secretKey);
                return Convert.ToBase64String(secretKey);
            }
        }

        public string GenerateToken(string id, string userType)
        {
            var claims = new[]
            {
                new Claim("id", id),
                new Claim("userType", userType),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _issuer,
                audience: _audience,
                claims: claims,
                expires: DateTime.Now.AddMinutes(7),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public string RefreshToken(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadToken(token) as JwtSecurityToken;

            //if (jwtToken == null || jwtToken.ValidTo > DateTime.UtcNow)
            if (jwtToken == null || jwtToken.ValidTo < DateTime.UtcNow)
                throw new SecurityTokenException("Invalid token");

            var idClaim = jwtToken.Claims.First(claim => claim.Type == "id");
            var userTypeClaim = jwtToken.Claims.First(claim => claim.Type == "userType");
            return GenerateToken(idClaim.Value, userTypeClaim.Value);
        }

        public void SetTokenCookie(HttpResponse response, string token)
        {
            response.Cookies.Append("token", token, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTimeOffset.UtcNow.AddDays(7)
            });
        }
    }
}
