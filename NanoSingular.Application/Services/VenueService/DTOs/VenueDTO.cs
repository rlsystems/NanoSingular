using NanoSingular.Application.Common.Marker;

namespace NanoSingular.Application.Services.VenueService.DTOs
{
    public class VenueDTO : IDto
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public DateTime CreatedOn { get; set; }

    }
}

