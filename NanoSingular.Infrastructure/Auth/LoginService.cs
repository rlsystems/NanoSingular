using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using NanoSingular.Infrastructure.Identity;
using NanoSingular.Infrastructure.Auth.DTOs;
using NanoSingular.Application.Common.Wrapper;
using Microsoft.AspNetCore.Authentication;

// service for authenticating users using cookies for Razor/MVC
namespace NanoSingular.Infrastructure.Auth
{
    public class LoginService : ILoginService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;


        public LoginService(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager
            )
        {
            _userManager = userManager;
            _signInManager = signInManager;
           
        }

        public async Task<Response> LoginAsync(TokenRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);

            if (user == null)
                return Response.Fail("Invalid user");

            if (user.IsActive == false)
                return Response.Fail("User account deactivated");

            var passwordIsCorrect = await _userManager.CheckPasswordAsync(user, request.Password);

            if (!passwordIsCorrect)
            {
                return Response.Fail("Incorrect Password");
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

            return Response.Success();

        }

    }
}


