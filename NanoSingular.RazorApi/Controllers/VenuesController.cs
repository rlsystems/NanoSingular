using Microsoft.AspNetCore.Mvc;
using NanoSingular.Application.Services.VenueService;

namespace NanoSingular.RazorApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VenuesController : ControllerBase
    {
        private readonly IVenueService _venueService;

        public VenuesController(IVenueService venueService)
        {
            _venueService = venueService;
        }

        [HttpGet] 
        public async Task<IActionResult> GetAllVenues()
        {
            var result = await _venueService.GetAllVenuesAsync();
            return Ok(result.Data);
        }


    }
}
