using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NanoSingular.Infrastructure.Identity;
using NanoSingular.Infrastructure.Identity.DTOs;

namespace NanoSingular.RazorApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IdentityController : ControllerBase
    {
        private readonly IIdentityService _identityService;

        public IdentityController(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        [HttpGet] // Get User List (admin-level permissions)
        public async Task<IActionResult> GetUsersAsync()
        {
            var usersList = await _identityService.GetUsersAsync();
            return Ok(usersList.Data);
        }

        [Authorize(Roles = "root, admin")]
        [HttpPost("register")] // Register a new user (admin-level permissions)
        public async Task<IActionResult> RegisterAsync(RegisterUserRequest request)
        {
            var result = await _identityService.RegisterAsync(request);
            return Ok(result);
        }

        [Authorize(Roles = "root, admin")]
        [HttpPut("user/{id}")] // Update a user (admin-level permissions)
        public async Task<IActionResult> UpdateUserAsync(UpdateUserRequest request, Guid id)
        {
            var result = await _identityService.UpdateUserAsync(request, id);
            return Ok(result);
        }

        [Authorize(Roles = "root,admin")]
        [HttpDelete("user/{id}")] // Delete a user (admin-level persmissions) (soft-delete)
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            var userId = await _identityService.DeleteUserAsync(id);
            return Ok(userId);
        }
    }
}