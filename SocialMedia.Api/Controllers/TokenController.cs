using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SocialMedia.Core.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SocialMedia.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public TokenController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost]
        public IActionResult Authentication(UserLogin userLogin)
        {
            //Si es usuario valido
            var token = GenerateToken(userLogin);

            return Ok(new { token });
        }

        private string GenerateToken(UserLogin userLogin)
        {
            string secretKey = _configuration["Authentication:SecretKey"];

            //Header
            var symmetricSecurityKey =
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var signingCredentials =
                new SigningCredentials(symmetricSecurityKey,
                    SecurityAlgorithms.HmacSha256);
            var header = new JwtHeader(signingCredentials);

            //Body Payload (Claims)
            var claims = new[]
            {
                new Claim("Name", "Juan Perez"),
                new Claim(ClaimTypes.Email, "jperez@correo.com"),
                new Claim(ClaimTypes.Role, "Administrator")
            };
            var payload = new JwtPayload(
                issuer: _configuration["Authentication:Issuer"],
                audience: _configuration["Authentication:Audience"],
                claims: claims,
                notBefore: DateTime.UtcNow,
                expires: DateTime.UtcNow.AddMinutes(2)
                );

            //Firma
            var token = new JwtSecurityToken(header, payload);

            //Serializar el token
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
