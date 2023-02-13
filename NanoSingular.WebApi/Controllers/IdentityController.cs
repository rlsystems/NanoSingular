using NanoSingular.Infrastructure.Identity;
using NanoSingular.Infrastructure.Identity.DTOs;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


// Identity Controller
namespace NanoSingular.WebApi.Controllers
{ 
    [ApiController]
    [Route("api/[controller]")]
    public  class IdentityController : ControllerBase
    {

        private readonly IIdentityService _identityService;

        public IdentityController(IIdentityService identityService)
        {
            _identityService = identityService;
        }
        
        [Authorize(Roles = "root, admin")]      
        [HttpGet] // Get User List (admin-level permissions)
        public async Task<IActionResult> GetUsersAsync()
        {
            var usersList = await _identityService.GetUsersAsync();
            return Ok(usersList);
        }

        [Authorize(Roles = "root, admin")]
        [Route("UserListPaginated")] // Get Paginated/Filtered User List (admin-level permissions)
        [HttpPost]
        public async Task<IActionResult> GetUsersAsync(UserListFilter filter)
        {       
            var usersList = await _identityService.GetUsersPaginatedAsync(filter); 
            return Ok(usersList);
        }


        [Authorize(Roles = "root, admin, editor, basic")]
        [HttpGet("profile")] // Get your own profile (basic user permissions)
        public async Task<IActionResult> GetProfileDetailsAsync()
        {
            var profile = await _identityService.GetProfileDetailsAsync();

            return Ok(profile);
        }

        [Authorize(Roles = "root, admin")]
        [HttpGet("user/{id}")] // Get User Details (admin-level permissions) -- ie: clicking edit in the user list
        public async Task<IActionResult> GetUserDetailsAsync(Guid id)
        {
            var profile = await _identityService.GetUserDetailsAsync(id);

            return Ok(profile);
        }

        [Authorize(Roles = "root, admin")]
        [HttpPost("register")] // Register a new user (admin-level permissions)
        public async Task<IActionResult> RegisterAsync([FromBody] RegisterUserRequest request)
        {
            var result = await _identityService.RegisterAsync(request);
            return Ok(result);       
        }

        [Authorize(Roles = "root, admin, editor, basic")]
        [HttpPut("profile")] // Update your profile (basic-level permissions)
        public async Task<IActionResult> UpdateProfileAsync([FromForm] UpdateProfileRequest request)
        {
            var result = await _identityService.UpdateProfileAsync(request);
            return Ok(result);
        }


        [Authorize(Roles = "root, admin, editor, basic")]
        [HttpPut("preferences")] // Update your preferences (basic-level permissions)
        public async Task<IActionResult> UpdateProfilePreferencesAsync(UpdatePreferencesRequest request)
        {
            var result = await _identityService.ChangePreferencesAsync(request);
            return Ok(result);
        }

        [Authorize(Roles = "root, admin")]
        [HttpPut("user/{id}")] // Update a user (admin-level permissions)
        public async Task<IActionResult> UpdateUserAsync(UpdateUserRequest request, Guid id)
        {
          
            var result = await _identityService.UpdateUserAsync(request, id);
            return Ok(result);
        }

        [HttpPut("change-password")] // Update your password (basic-level permissions)
        [Authorize(Roles = "root, admin, editor, basic")]
        public async Task<IActionResult> ChangePasswordAsync(ChangePasswordRequest request)
        {
            return Ok(await _identityService.ChangePasswordAsync(request));
        }


        [Authorize(Roles = "root,admin")]
        [HttpDelete("user/{id}")] // Delete a user (admin-level persmissions) (soft-delete)
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            var userId = await _identityService.DeleteUserAsync(id);
            return Ok(userId);
        }

   
        [AllowAnonymous] // Forgot Password / Password Reset -- tenant Id will be from header / subdomain
        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPasswordAsync(ForgotPasswordRequest request)
        {
            string origin = GenerateOrigin();
            var result = await _identityService.ForgotPasswordAsync(request, origin);

            return Ok(result);
        }

        [AllowAnonymous] // tenant id will be from header / subdomain
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPasswordAsync(ResetPasswordRequest request)
        {
            return Ok(await _identityService.ResetPasswordAsync(request));
        }

        
        private string GenerateOrigin() // Helper method to return origin URL
        {
            string baseUrl = $"{this.Request.Scheme}://{this.Request.Host.Value}{this.Request.PathBase.Value}";
            string origin = string.IsNullOrEmpty(Request.Headers["origin"].ToString()) ? baseUrl : Request.Headers["origin"].ToString();
            return origin;
        }
    }
}