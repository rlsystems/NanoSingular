using NanoSingular.Infrastructure.Auth;
using NanoSingular.Infrastructure.Auth.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// Login controller, returns JWT tokens to authenticated users

namespace NanoSingular.WebApi.Controllers
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    [ApiController]
    public class TokensController : ControllerBase
    {
    
        private readonly ITokenService _tokenService;

        public TokensController(ITokenService tokenService)
        {
            _tokenService = tokenService;
        }


        [AllowAnonymous]
        [HttpPost] // Get Token (Login) -- must provide tenant Id in header / subdomain
        public async Task<IActionResult> GetTokenAsync([FromBody] TokenRequest request)
        {
            var response = await _tokenService.GetTokenAsync(request);
            return Ok(response);    
        }


        // Refresh token endpoint
        // -- add here if needed
     
    }
}
