using NanoSingular.Application.Services.VenueService;
using NanoSingular.Application.Services.VenueService.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// Sample business entity controller (venues) with CRUD operations

namespace NanoSingular.WebApi.Controllers
{

    [Route("api/[controller]")]

    [ApiController]
    public class VenuesController : ControllerBase
    {
        private readonly IVenueService _venueService; // inject the venue service

        public VenuesController(IVenueService venueService)
        {
            _venueService = venueService; 
        }


        [Authorize(Roles = "root, admin, editor, basic")]
        [HttpGet] // Get Venues - Full List
        public async Task<IActionResult> GetAllVenuesAsync()
        {
            var venues = await _venueService.GetAllVenuesAsync();

            return Ok(venues);
        }

        [Authorize(Roles = "root, admin, editor, basic")]
        [HttpPost("VenueListPaginated")] // Get Venues - Paginated/Filtered List (Alternatly, could use GET with parameters [FromQuery])
        public async Task<IActionResult> GetAllVenuesPaginatedAsync(VenueListFilter filter)
        {
            var venues = await _venueService.GetAllVenuesPaginatedAsync(filter);
            
            return Ok(venues);
        }

        [Authorize(Roles = "root, admin, editor, basic")]
        [HttpGet("{id}")] // Get a single venue by Id
        public async Task<IActionResult> GetVenue(Guid id)
        {
            var venues = await _venueService.GetVenue(id);

            return Ok(venues);
        }


        [Authorize(Roles = "root, admin, editor")]
        [HttpPost] // Create a venue (editor level users and above)
        public async Task<IActionResult> CreateAsync(CreateVenueRequest request)
        {

            try
            {
                var result = await _venueService.CreateVenueAsync(request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        
        [Authorize(Roles = "root,admin, editor")]
        [HttpPut("{id}")] // Edit a venue (editor level users and above)
        public async Task<IActionResult> UpdateAsync(UpdateVenueRequest request, Guid id)
        {
            try
            {
                var result = await _venueService.UpdateVenueAsync(request, id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "root,admin, editor")]
        [HttpDelete("{id}")] // Delete a venue (editor level users and above)
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            var venueId = await _venueService.DeleteVenueAsync(id);
            return Ok(venueId);
        }


    }
}
