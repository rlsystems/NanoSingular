using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NanoSingular.Application.Common;

namespace NanoSingular.RazorApi.Pages.Users
{
    public class IndexModel : PageModel
    {
        private readonly ICurrentTenantUserService _currentTenantUserService;

        public string? CurrentUserId { get; set; }

        public IndexModel(ICurrentTenantUserService currentTenantUserService)
        {
            _currentTenantUserService = currentTenantUserService;
        }

        public IActionResult OnGet()
        {
            CurrentUserId = _currentTenantUserService.UserId;
            return Page();
        }
    }
}