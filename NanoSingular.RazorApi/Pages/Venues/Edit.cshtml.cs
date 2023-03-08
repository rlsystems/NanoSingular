using AutoMapper;
using MathNet.Numerics.Distributions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using NanoSingular.Application.Services.VenueService;
using NanoSingular.Application.Services.VenueService.DTOs;
using System;

namespace NanoSingular.RazorApi.Pages.Venues
{
    public class EditModel : PageModel
    {

        private readonly IVenueService _venueService;
        private readonly IMapper _mapper;

        public EditModel(IVenueService venueService, IMapper mapper)
        {
            _venueService = venueService;
            _mapper = mapper;
            UpdateVenueRequest = new UpdateVenueRequest();
        }


        [BindProperty]
        public UpdateVenueRequest UpdateVenueRequest { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            if (id == Guid.Empty)
            {
                return NotFound();
            }

            var result = await _venueService.GetVenue(id);
      
            if(result.Data == null)
                return NotFound();


            UpdateVenueRequest = _mapper.Map(result.Data, UpdateVenueRequest); // map response to dto

            return Page();


        }


        public async Task<IActionResult> OnPostAsync(Guid id, string action)
        {

            switch (action)
            {
                case "delete":
                    await _venueService.DeleteVenueAsync(id);
                    return RedirectToPage("./Index");
                case "save":
                    if (ModelState.IsValid)
                    {
                        await _venueService.UpdateVenueAsync(UpdateVenueRequest, id);
                        return RedirectToPage("./Index");
                    }
                    return Page();
            }

            return Page();


        }
    }
}
