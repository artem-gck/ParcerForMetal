using Microsoft.AspNetCore.Mvc;
using Parser.Serviсes.Models.Context;
using Parser.Serviсes;
using Parser.Serviсes.Models.Login;
using System.Security.Claims;
using Parser.Serviсes.Models;
using AutoMapper;
using Parser.Serviсes.ViewModel;

namespace Parser.Controllers
{
    [Route("api/authentication")]
    [ApiController]
    public class AuthController : Controller
    {
        private readonly IAuthManager _authService;
        private readonly ITokenManager _tokenService;
        private readonly string _headerName;

        public AuthController(IAuthManager authService, ITokenManager tokenService, IConfiguration configuration)
            => (_authService, _tokenService, _headerName) = (authService, tokenService, configuration.GetSection("HeaderName").Value);

        [HttpPost, Route("login")]
        public async Task<ActionResult<TokenApiModel>> Login([FromBody] User loginModel)
        {
            if (loginModel == null)
                return BadRequest("Invalid client request");

            var tokens = await _authService.Login(loginModel);

            if (tokens is null)
                return Unauthorized();

            return tokens;
        }

        [HttpGet]
        public async Task<ActionResult<UserViewModel>> GetUser(string login)
        {
            if (await _tokenService.CheckAccessKey(Request.Headers[_headerName].ToString()))
            {
                var userInfo = await _authService.GetUser(login);

                var config = new MapperConfiguration(cfg => cfg.CreateMap<UserInfo, UserViewModel>());
                var mapper = new Mapper(config);

                var userViewModel = mapper.Map<UserViewModel>(userInfo);

                return userViewModel;
            }
            else
                return Unauthorized();
        }
    }
}
