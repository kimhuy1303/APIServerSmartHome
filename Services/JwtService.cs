using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace APIServerSmartHome.Services
{
    public class JwtService
    {
        private readonly IConfiguration _configuration;
        public JwtService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public string generateJwtAccessToken(int id, string username)
        {
            var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]!));
            var tokenHandler = new JwtSecurityTokenHandler();
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, username.ToString()),
                new Claim("UserId", id.ToString())
            };
            var signin = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(60),
                Audience = _configuration["jwt:Audience"],
                Issuer = _configuration["jwt:Issuer"],
                SigningCredentials = signin
            };
            var tokenvalue = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(tokenvalue);
        }
    }
}
