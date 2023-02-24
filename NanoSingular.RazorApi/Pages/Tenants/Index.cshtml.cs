using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace NanoSingular.RazorApi.Pages.Tenants
{
    [Authorize(Roles = "root")]
    public class IndexModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}
