
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
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

        public void OnGet()
        {
        }


        public async Task OnPostAsync(CreateVenueRequest request)
        {

            var CreateVenueRequest = new CreateVenueRequest();
            CreateVenueRequest.Name = Request.Form["venueName"];
            CreateVenueRequest.Description = Request.Form["venueDescription"];

            await _venueService.CreateVenueAsync(CreateVenueRequest);
        }

        //public async Task SaveForm(CreateVenueRequest request)
        //{

        //    var test = "test";
        //}

    }
}
