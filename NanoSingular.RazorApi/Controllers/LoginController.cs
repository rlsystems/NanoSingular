using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NanoSingular.Application.Common.Wrapper;
using NanoSingular.Infrastructure.Auth;
using NanoSingular.Infrastructure.Auth.DTOs;
using NanoSingular.Infrastructure.Identity;
using NanoSingular.Infrastructure.Identity.DTOs;
using NanoSingular.RazorApi.Services;
using System.Security.Claims;

namespace NanoSingular.RazorApi.Controllers
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly ILoginService _loginService;

        public LoginController(ILoginService loginService)
        {
            _loginService = loginService;
        }


        [HttpPost]
        public async Task<IActionResult> Login(TokenRequest request)
        {

            var response = await _loginService.LoginAsync(request);

            return Ok(response);

        }
    }




}
