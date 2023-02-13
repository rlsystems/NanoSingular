using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NanoSingular.Application.Common;
using NanoSingular.Infrastructure.Identity;
using NanoSingular.RazorApi.Services;
using System.Security.Claims;

namespace NanoSingular.RazorApi.Pages.Users
{
    public class IndexModel : PageModel
    {
        //private readonly ICurrentTenantUserService _currentUserService;

        public IndexModel() {
            //_currentUserService = currentUserService;
        }

        public string CurrentUserId { get; set; }

        public IActionResult OnGet()
        {
            //need to add claims, user ID and tenant :D
            //CurrentUserId = _currentUserService.UserId;
            CurrentUserId = "test2";
            return Page();
        }
       

    }
}
