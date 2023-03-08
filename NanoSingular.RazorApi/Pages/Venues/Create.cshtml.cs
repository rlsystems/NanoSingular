
using CloudinaryDotNet;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using NanoSingular.Application.Services.VenueService;
using NanoSingular.Application.Services.VenueService.DTOs;

namespace NanoSingular.RazorApi.Pages.Venues
{
    public class CreateModel : PageModel
    {
        private readonly IVenueService _venueService;

        public CreateModel(IVenueService venueService)
        {
            _venueService = venueService;
        }

        [BindProperty]
        public CreateVenueRequest CreateVenueRequest { get; set; }


        public void OnGet()
        {
        }


        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                await _venueService.CreateVenueAsync(CreateVenueRequest);
                return RedirectToPage("./Index");

            }

            return Page();
        }
    }
}
