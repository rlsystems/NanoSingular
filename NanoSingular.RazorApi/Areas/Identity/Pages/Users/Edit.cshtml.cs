using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NanoSingular.Infrastructure.Identity;
using NanoSingular.Infrastructure.Identity.DTOs;

namespace NanoSingular.RazorApi.Areas.Identity.Pages.Users
{
    public class EditModel : PageModel
    {
        private readonly IIdentityService _identityService;

        public EditModel(IIdentityService identityService)
        {
            _identityService = identityService;
        }
        public UserDto User { get; set; }


        public async Task<IActionResult> OnGetAsync(Guid userId)
        {
            var response = await _identityService.GetUserDetailsAsync(userId);
            User = response.Data;
            return Page();
        }
    }
}
