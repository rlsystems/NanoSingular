using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NanoSingular.Application.Services.VenueService.DTOs;
using NanoSingular.Application.Services.VenueService;
using NanoSingular.Infrastructure.Identity.DTOs;
using NanoSingular.Infrastructure.Identity;

namespace NanoSingular.RazorApi.Pages.Profile
{
    public class _BasicInfoModel : PageModel
    {


        private readonly IIdentityService _identityService;
        private readonly IMapper _mapper;

        public _BasicInfoModel(IIdentityService identityService, IMapper mapper)
        {
            _identityService = identityService;
            _mapper = mapper;
            updateProfileRequest = new UpdateProfileRequest();
        }

        [BindProperty]
        public UpdateProfileRequest updateProfileRequest { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {


            var result = await _identityService.GetProfileDetailsAsync();

            if (result.Data == null)
                return NotFound();


            updateProfileRequest = _mapper.Map(result.Data, updateProfileRequest); // map response to dto

            return Page();

        }
    }
}
