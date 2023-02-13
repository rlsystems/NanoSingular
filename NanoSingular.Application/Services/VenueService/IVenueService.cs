using NanoSingular.Application.Common.Marker;
using NanoSingular.Application.Common.Wrapper;
using NanoSingular.Application.Services.VenueService.DTOs;


namespace NanoSingular.Application.Services.VenueService
{
    public interface IVenueService : ITransientService
    {
        Task<Response<IEnumerable<VenueDTO>>> GetAllVenuesAsync();
        Task<PaginatedResponse<VenueDTO>> GetAllVenuesPaginatedAsync(VenueListFilter filter);
        Task<Response<VenueDTO>> GetVenue(Guid id);
        Task<Response<VenueDTO>> CreateVenueAsync(CreateVenueRequest modal);
        Task<Response<VenueDTO>> UpdateVenueAsync(UpdateVenueRequest modal, Guid id);
        Task<Response<Guid>> DeleteVenueAsync(Guid id);

    }
}
