using Microsoft.AspNetCore.Mvc;
using Parser.Serviсes.Models.Context;
using Parser.Serviсes;
using Parser.Serviсes.Models.Login;
using System.Security.Claims;
using Parser.Serviсes.Models;

namespace Parser.Controllers
{
    [Route("api/authentication")]
    [ApiController]
    public class AuthController : Controller
    {
        private readonly IAuthManager _service;

        public AuthController(IAuthManager service)
            => _service = service;

        [HttpPost, Route("login")]
        public async Task<ActionResult<TokenApiModel>> Login([FromBody] User loginModel)
        {
            if (loginModel == null)
                return BadRequest("Invalid client request");

            var tokens = await _service.Login(loginModel);

            if (tokens is null)
                return Unauthorized();

            return tokens;
        }
    }
}
