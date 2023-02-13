using NanoSingular.Application.Common.Marker;
using FluentValidation;


namespace NanoSingular.Application.Services.VenueService.DTOs
{
    public class UpdateVenueRequest : IDto
    {

        public string Name { get; set; }
        public string Description { get; set; }
   
    }

    public class UpdateVenueValidator : AbstractValidator<UpdateVenueRequest>
    {
        public UpdateVenueValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.Description).NotEmpty();

        }
    }
}

