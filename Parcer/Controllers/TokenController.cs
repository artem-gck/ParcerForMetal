using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Parser.Serviсes;
using Parser.Serviсes.Models;
using Parser.Serviсes.Models.Context;

namespace Parser.Controllers
{
    [Route("api/token")]
    [ApiController]
    public class TokenController : Controller
    {
        private readonly ITokenManager _service;

        public TokenController(MetalContext userContext, ITokenManager tokenService)
        {
            _service = tokenService ?? throw new ArgumentNullException(nameof(tokenService));
        }

        [HttpPost]
        [Route("refresh")]
        public async Task<ActionResult<TokenApiModel>> Refresh(TokenApiModel tokenApiModel)
        {
            if (tokenApiModel is null)
                return BadRequest("Invalid client request");

            var newTokens = await _service.Refresh(tokenApiModel);

            return newTokens is not null ? newTokens : NotFound();
        }

        //[HttpPost, Authorize]
        //[Route("revoke")]
        //public IActionResult Revoke()
        //{
        //    var username = User.Identity.Name;
        //    var user = userContext.User.SingleOrDefault(u => u.Login == username);

        //    if (user == null)
        //        return BadRequest();
            
        //    user.RefreshToken = null;
        //    userContext.SaveChanges();
            
        //    return NoContent();
        //}
    }
}
