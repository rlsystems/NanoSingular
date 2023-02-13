using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Options;
using NanoSingular.Infrastructure.Identity;
using NanoSingular.Infrastructure.Auth.DTOs;
using NanoSingular.Application.Common.Wrapper;

// service for authenticating users and issuing a JWT token
namespace NanoSingular.Infrastructure.Auth
{
    public class TokenService : ITokenService
    {
        private readonly JWTSettings _jwtSettings;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;


        public TokenService(
            IOptions<JWTSettings> JWTSettings,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager
            )
        {
            _jwtSettings = JWTSettings.Value;
            _userManager = userManager;
            _signInManager = signInManager;
           
        }

        public async Task<Response<TokenResponse>> GetTokenAsync(TokenRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email.Normalize());

            if (user == null)
                return Response<TokenResponse>.Fail("Invalid user");

            if (user.IsActive == false)
                return Response<TokenResponse>.Fail("User account deactivated");

            var result = await _signInManager.PasswordSignInAsync(user.UserName, request.Password, false, lockoutOnFailure: false);

            if (!result.Succeeded)
                return Response<TokenResponse>.Fail("Unauthorized");

            if (!user.EmailConfirmed)
                return Response<TokenResponse>.Fail("Unauthorized");

            JwtSecurityToken jwtSecurityToken = await GenerateJWTToken(user);


            var response =  new TokenResponse()
            {
                Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
                RefreshTokenExpiryTime = jwtSecurityToken.ValidTo,
                RefreshToken = null,
               
            };
            return Response<TokenResponse>.Success(response);

        }

        
        private async Task<JwtSecurityToken> GenerateJWTToken(ApplicationUser user) 
        {
            var roles = await _userManager.GetRolesAsync(user);

            var roleClaims = new List<Claim>();
            for (int i = 0; i < roles.Count; i++)
            {
                roleClaims.Add(new Claim("roles", roles[i]));
            }


            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("uid", user.Id),
                new Claim("tenant", "mytest"),
            }
            .Union(roleClaims);

            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            // create JWT token with claims (roles, userid, tenantid) that expires after time set in appsettings.json
            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwtSettings.DurationInMinutes),
                signingCredentials: signingCredentials);

            return jwtSecurityToken;
        }
    }
}


