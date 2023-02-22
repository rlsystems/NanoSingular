using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NanoSingular.Application.Common.Wrapper;
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

        private readonly SignInManager<ApplicationUser> _signInManager; //this was
        private readonly UserManager<ApplicationUser> _userManager;

        public LoginController(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager)
        {

            _signInManager = signInManager;
            _userManager = userManager;
        }


        [HttpPost()]
        public async Task<Response<TokenResponse>> Login(TokenRequest request)
        {


            var user = await _userManager.FindByEmailAsync(request.Email);

            if (user == null)
                return Response<TokenResponse>.Fail("Invalid user");

            if (user.IsActive == false)
                return Response<TokenResponse>.Fail("User account deactivated");

            var passwordIsCorrect = await _userManager.CheckPasswordAsync(user, request.Password);

            if (!passwordIsCorrect)
            {
                return Response<TokenResponse>.Fail("Incorrect Password");
            }

            var customClaims = new[]{
                    new Claim("tenant", user.TenantId) 
                };

            var claimsPrincipal = await _signInManager.CreateUserPrincipalAsync(user);
            if (claimsPrincipal.Identity is ClaimsIdentity claimsIdentity)
            {
                claimsIdentity.AddClaims(customClaims);
            }


            await _signInManager.Context.SignInAsync(IdentityConstants.ApplicationScheme, claimsPrincipal);

            return Response<TokenResponse>.Success();

        }
    }




}
