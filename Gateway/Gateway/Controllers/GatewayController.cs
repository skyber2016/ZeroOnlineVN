using Gateway.Models;
using Gateway.Services;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Gateway.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GatewayController : ControllerBase
    {
        private readonly ILogger<GatewayController> _logger;

        public GatewayController(ILogger<GatewayController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        public IActionResult Post([FromBody] GatewayModel request)
        {
            var decrypt = CryptoService.Decrypt(request);
            if(decrypt == null)
            {
                return BadRequest();
            }

            var res = CryptoService.Encrypt(decrypt, new
            {
                code = HttpStatusCode.OK,
                message = "hello world"
            });
            return Ok(res);
        }
    }
}
