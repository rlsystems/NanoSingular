using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;


namespace NanoSingular.RazorApi.Areas.Identity.Pages.UsersAjax
{
    [Authorize]
    public class IndexModel : PageModel
    {


        public IActionResult OnGet()
        {

            return Page();
        }
    }
}
