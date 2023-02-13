using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NanoSingular.Infrastructure.Identity;
using NanoSingular.Infrastructure.Identity.DTOs;

namespace NanoSingular.RazorApi.Areas.Identity.Pages.Users
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly IIdentityService _identityService;

        public IndexModel(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        public IEnumerable<UserDto> Users { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var response = await _identityService.GetUsersAsync();
            Users = response.Data;
            return Page();
        }
    }
}
