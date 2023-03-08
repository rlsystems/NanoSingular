
using NanoSingular.Domain.Entities;
using NanoSingular.Infrastructure.Identity;
using NanoSingular.Infrastructure.Identity.DTOs;
using NanoSingular.Application.Services.VenueService.DTOs;
using AutoMapper;


// Automapper mapping configurations
namespace NanoSingular.Infrastructure.Mapper
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {

            // app Users
            CreateMap<ApplicationUser, UserDto>();
            CreateMap<UpdateProfileRequest, ApplicationUser>();
            CreateMap<UpdatePreferencesRequest, ApplicationUser>();
            CreateMap<UpdateUserRequest, ApplicationUser>();
            CreateMap<RegisterUserRequest, ApplicationUser>()
                .ForMember(x => x.UserName, o => o.MapFrom(s => s.Email));

            // venues
            CreateMap<Venue, VenueDTO>();
            CreateMap<CreateVenueRequest, Venue>();
            CreateMap<UpdateVenueRequest, Venue>();

            CreateMap<VenueDTO, UpdateVenueRequest>();


            // add new entity mappings here...


        }
    }
}
