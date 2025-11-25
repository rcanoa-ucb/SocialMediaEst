using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SocialMedia.Core.Entities;
using SocialMedia.Core.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;

namespace SocialMedia.Api.Controllers
{
    [Route("api/Login")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly ISecurityServices _securityServices;
        public LoginController(IConfiguration configuration,
            ISecurityServices securityServices)
        {
            _configuration = configuration;
            _securityServices = securityServices;
        }

        [HttpPost]
        [Route("Insertar")]
        public async Task<IActionResult> Post()
        {
            //UserLogin userLogin
            try
            {
                ////Si es usuario valido
                //var validation = await IsValidUser(userLogin);
                //if (validation.Item1)
                //{
                //    var token = GenerateToken(validation.Item2);
                //    return Ok(new { token });
                //}

                //return NotFound("Credenciales no válidas");

                return Ok("Soy Post");
            }
            catch (Exception err)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, err.Message);
            }
        }

        [HttpGet]
        [Route("Obtener")]
        public async Task<IActionResult> Get()
        {
            try
            {
                //var res = await _securityServices.GetLoginByCredentials(new UserLogin() { User = "admin", Password = "admin" });

                //return Ok(res);

                return Ok("Soy Get");
            }
            catch (Exception err)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, err.Message);
            }
        }

        private string GenerateToken(Security security)
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
                new Claim("Name", security.Name),
                new Claim("Login", security.Login),
                new Claim(ClaimTypes.Role, security.Role.ToString())
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

        private async Task<(bool, Security)> IsValidUser(UserLogin userLogin)
        {
            var user = await _securityServices.GetLoginByCredentials(userLogin);
            return (user != null, user);
        }
    }
}
