using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace NanoSingular.RazorApi.Pages.Authentication
{
    [AllowAnonymous]

    public class Unauthorized : PageModel
    {
        public void OnGet()
        {
        }
    }
}
