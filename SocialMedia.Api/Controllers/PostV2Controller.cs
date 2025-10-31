using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace SocialMedia.Api.Controllers
{
    [Route("api/v{version:ApiVersion}/post")]
    [ApiVersion("2.0")]
    [ApiController]
    public class PostV2Controller : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(new 
            {
                Version = 2.0,
                Message = "Estoy en la version 2"
            });
        }
    }
}
