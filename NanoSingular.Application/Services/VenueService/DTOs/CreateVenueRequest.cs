using NanoSingular.Application.Common.Marker;
using FluentValidation;


namespace NanoSingular.Application.Services.VenueService.DTOs
{
    public class CreateVenueRequest : IDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }

    public class CreateVenueValidator : AbstractValidator<CreateVenueRequest>
    {
        public CreateVenueValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.Description).NotEmpty();
        }
    }
}
