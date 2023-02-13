using System.Linq.Expressions;
using AutoMapper;
using NanoSingular.Application.Services.VenueService.DTOs;
using NanoSingular.Domain.Entities;
using NanoSingular.Application.Common.Wrapper;
using NanoSingular.Application.Common;
using NanoSingular.Application.Common.Specification;

// Venue service - sample app service
namespace NanoSingular.Application.Services.VenueService
{
    public class VenueService : IVenueService
    {
        private readonly IRepositoryAsync _repository; // inject repository / mapper
        private readonly IMapper _mapper;

        public VenueService(IRepositoryAsync repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        // Get full List (for react table -- not used in demo)
        public async Task<Response<IEnumerable<VenueDTO>>> GetAllVenuesAsync()
        {
            var specification = new BaseSpecification<Venue>();
            specification.AddOrderByDescending(x => x.CreatedOn);

            var list = await _repository.GetListAsync<Venue, VenueDTO>(specification); // entity mapped to dto

            return Response<IEnumerable<VenueDTO>>.Success(list);
        }

        // Get paginated / filtered list (for server table)
        public async Task<PaginatedResponse<VenueDTO>> GetAllVenuesPaginatedAsync(VenueListFilter filter)
        {

            Expression<Func<Venue, bool>> filterExpression = null;
            if (!String.IsNullOrWhiteSpace(filter.Keyword))
            {
                filterExpression = (x => x.Name.ToLower().Contains(filter.Keyword.ToLower()));
            }

            var specification = new BaseSpecification<Venue>(filterExpression);
            specification.AddOrderByDescending(x => x.CreatedOn);

            var pagedResponse = await _repository.GetPaginatedResultsAsync<Venue, VenueDTO>(filter.PageNumber, filter.PageSize, specification); // paginated response, entity mapped to dto
            return pagedResponse;

        }

        // Get single venue by Id 
        public async Task<Response<VenueDTO>> GetVenue(Guid id)
        {
            try
            {
                var venueDto = await _repository.GetByIdAsync<Venue, VenueDTO>(id);

                return Response<VenueDTO>.Success(venueDto);
            }
            catch (Exception ex)
            {
                return Response<VenueDTO>.Fail(ex.Message);
            }
        }

        // Create new venue
        public async Task<Response<VenueDTO>> CreateVenueAsync(CreateVenueRequest request)
        {
            var specification = new BaseSpecification<Venue>(x => x.Name == request.Name); // Check if venue exists by Id from database
            bool venueExists = await _repository.ExistsAsync<Venue>(specification);
            if (venueExists)
                return Response<VenueDTO>.Fail("Venue already exists");

            
            Venue newVenue = _mapper.Map(request, new Venue()); // Map request to new domain entity object

            try
            {
                Venue response = await _repository.CreateAsync(newVenue); // create venue 
                await _repository.SaveChangesAsync(); // save changes to db
                VenueDTO venueDto = _mapper.Map(response, new VenueDTO()); // map response to dto

                return Response<VenueDTO>.Success(venueDto); // return dto

            }
            catch (Exception ex)
            {
                return Response<VenueDTO>.Fail(ex.Message);               
            }
        }
 
        // Update venue
        public async Task<Response<VenueDTO>> UpdateVenueAsync(UpdateVenueRequest request, Guid id)
        {
            Venue venueInDb = await _repository.GetByIdAsync<Venue>(id); // Check if exists
            if (venueInDb == null)
                return Response<VenueDTO>.Fail("Not Found");

            
            Venue updatedVenue = _mapper.Map(request, venueInDb); // Map request to domain object

            try
            {    
                Venue response = await _repository.UpdateAsync(updatedVenue);  // update venue 
                await _repository.SaveChangesAsync(); // save changes to db
                VenueDTO venueDto = _mapper.Map(response, new VenueDTO()); // map response to dto

                return Response<VenueDTO>.Success(venueDto);

            }
            catch (Exception ex)
            {
                return Response<VenueDTO>.Fail(ex.Message);
            }
        }

        // Delete Venue
        public async Task<Response<Guid>> DeleteVenueAsync(Guid id)
        {
            try
            {
                var venue = await _repository.RemoveByIdAsync<Venue>(id);
                await _repository.SaveChangesAsync();

                return Response<Guid>.Success(venue.Id);
            }
            catch (Exception ex)
            {
                return Response<Guid>.Fail(ex.Message);
            }
       

        }
    }
}

