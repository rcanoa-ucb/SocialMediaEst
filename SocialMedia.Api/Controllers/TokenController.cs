using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SocialMedia.Core.Entities;
using SocialMedia.Services.Interfaces;
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
        private readonly ISecurityService _securityService;

        public TokenController(IConfiguration configuration,
            ISecurityService securityService)
        {
            _configuration = configuration;
            _securityService = securityService;
        }

        [HttpPost]
        public async Task<IActionResult> Login(UserLogin userLogin)
        {
            //Si es un usuario es válido generar el token
            var validation = await IsValidUser(userLogin);
            if (validation.Item1)
            {
                var token = GenerateToken(userLogin);
                return Ok(new { token });
            }

            return Unauthorized();
        }

        private async Task<(bool, Security)> IsValidUser(UserLogin userLogin)
        {
            var user = await _securityService.GetLoginByCredentials(userLogin);
            return (user != null, user);
        }

        private string GenerateToken(UserLogin userLogin)
        {
            //HEADER
            var symmetricSecurityKey =
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                    _configuration["Authentication:SecretKey"]));
            var signingCredentials =
                new SigningCredentials(symmetricSecurityKey,
                SecurityAlgorithms.HmacSha256);
            var header = new JwtHeader(signingCredentials);

            //PAYLOAD (Cuerpo)
            var claims = new[]
            {
                new Claim("Name", "Juan Perez"),
                new Claim(ClaimTypes.Email, "jperez@ucb.edu.bo"),
                new Claim(ClaimTypes.Role, "Administrator")
            };

            var payload =
                new JwtPayload(
                    //Quien emite el token (ej: https://api.ucb.edu.bo)
                    issuer: _configuration["Authentication:Issuer"],

                    //Quien va a recibir el token (ej: https://frontend.ucb.edu.bo)
                    audience: _configuration["Authentication:Audience"],

                    //los datos del usuario
                    claims: claims,

                    //Desde cuando es valido el token (ahora mismo)
                    notBefore: DateTime.UtcNow,

                    //Cuando expira
                    expires: DateTime.UtcNow.AddMinutes(2)
                    );

            //FIRMA
            var token =
                new JwtSecurityToken(header, payload);

            //Serializar el token
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}