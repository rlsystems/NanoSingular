using AutoMapper;
using Microsoft.EntityFrameworkCore;

using NanoSingular.Domain.Entities;
using NanoSingular.Infrastructure.Persistence.Contexts;
using NanoSingular.Application.Common.Wrapper;
using NanoSingular.Application.Common.Marker;
using NanoSingular.Application.Common;
using NanoSingular.Application.Common.Specification;

// Repository class
// -- this class should be used by all application services
// -- it provides abstraction from _context, it can return DTO-mapped lists with pagination
// -- use ISpecification (Base Specification) to pass criteria, includes, and sorting filters

namespace NanoSingular.Infrastructure.Persistence.Repository
{
    public class RepositoryAsync : IRepositoryAsync
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public RepositoryAsync(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        #region Get

        // Get all, return non-paginated list of Domain Entities
        public async Task<IEnumerable<T>> GetListAsync<T>(ISpecification<T> specification = null, CancellationToken cancellationToken = default) where T : BaseEntity
        {

            IQueryable<T> query = _context.Set<T>();
            if (specification != null)
            {
                query = SpecificationEvaluator<T>.GetQuery(_context.Set<T>().AsQueryable(), specification);
            }

            var result = await query.ToListAsync(cancellationToken);
            return result;
        }

        // Get all, return non-paginated list of Mapped Dtos
        public async Task<IEnumerable<TDto>> GetListAsync<T, TDto>(ISpecification<T> specification = null, CancellationToken cancellationToken = default) where T : BaseEntity
            where TDto : IDto
        {

            IQueryable<T> query = _context.Set<T>();
            if (specification != null)
            {
                query = SpecificationEvaluator<T>.GetQuery(_context.Set<T>().AsQueryable(), specification);
            }

            var list = await query.ToListAsync(cancellationToken);
            var result = _mapper.Map<List<TDto>>(list); // map results

            return result;
        }

        // Get by Id, return Domain Entity
        public async Task<T> GetByIdAsync<T>(Guid id, ISpecification<T> specification = null, CancellationToken cancellationToken = default) where T : BaseEntity
        {
            IQueryable<T> query = _context.Set<T>();
            if (specification != null)
            {
                query = SpecificationEvaluator<T>.GetQuery(_context.Set<T>().AsQueryable(), specification);
            }

            var entity = await query.Where(x => x.Id == id).FirstOrDefaultAsync(cancellationToken);

            if (entity != null)
            {
                return entity;
            }
            else
            {
                throw new Exception("Not Found");
            }

        }

        // Get by Id, return Mapped Dtos
        public async Task<TDto> GetByIdAsync<T, TDto>(Guid id, ISpecification<T> specification = null, CancellationToken cancellationToken = default)
            where T : BaseEntity
            where TDto : IDto
        {
            IQueryable<T> query = _context.Set<T>();
            if (specification != null)
            {
                query = SpecificationEvaluator<T>.GetQuery(_context.Set<T>().AsQueryable(), specification);
            }

            var entity = await query.Where(x => x.Id == id).FirstOrDefaultAsync(cancellationToken);

            if (entity != null)
            {
                var dto = _mapper.Map<TDto>(entity);
                return dto;
            }
            else
            {
                throw new Exception("Not Found");
            }

        }

        // Check if exists, return true/false
        public async Task<bool> ExistsAsync<T>(ISpecification<T> specification = null, CancellationToken cancellationToken = default)
        where T : BaseEntity
        {
            IQueryable<T> query = _context.Set<T>();
            if (specification != null)
            {
                query = SpecificationEvaluator<T>.GetQuery(_context.Set<T>().AsQueryable(), specification);
            }
            return await query.AnyAsync(cancellationToken);
        }

        #endregion Get

        #region Create

        // Create
        public async Task<T> CreateAsync<T>(T entity)
        where T : BaseEntity
        {
            await _context.Set<T>().AddAsync(entity);
            return entity;

        }

        // Create range, retun List of Guid
        public async Task<IList<Guid>> CreateRangeAsync<T>(IEnumerable<T> entity)
        where T : BaseEntity
        {
            await _context.Set<T>().AddRangeAsync(entity);
            return entity.Select(x => x.Id).ToList();
        }
        #endregion Create

        #region Update

        // Update
        public async Task<T> UpdateAsync<T>(T entity) 
            where T : BaseEntity
        {
            if (_context.Entry(entity).State == EntityState.Unchanged)
            {
                throw new Exception("Nothing to update");
            }

            T entityInDb = await _context.Set<T>().FindAsync(entity.Id);
            if (entityInDb != null)
            {
                _context.Entry(entityInDb).CurrentValues.SetValues(entity);
                return entity;
            }
            else
            {
                throw new Exception("Not Found");
            }

        }
        #endregion Update

        #region Remove

        // Remove
        public Task RemoveAsync<T>(T entity) where T : BaseEntity
        {
            _context.Set<T>().Remove(entity);
            return Task.CompletedTask;
        }

        // Remove by Id
        public async Task<T> RemoveByIdAsync<T>(Guid entityId)
        where T : BaseEntity
        {
            var entity = await _context.Set<T>().FindAsync(entityId);
            if (entity == null)
                throw new Exception("Not Found");

            _context.Set<T>().Remove(entity);

            return entity;
        }
        #endregion Remove

        #region Pagination

        // Get all/condition, return paginated list of Domain Entities
        public async Task<PaginatedResponse<T>> GetPaginatedResultsAsync<T>(int pageNumber, int pageSize = int.MaxValue, ISpecification<T> specification = null, CancellationToken cancellationToken = default) where T : BaseEntity
        {

            IQueryable<T> query = _context.Set<T>(); // build query
            if (specification != null)
            {
                query = SpecificationEvaluator<T>.GetQuery(_context.Set<T>().AsQueryable(), specification); 
            }

            var filteredList = await query.ToListAsync(cancellationToken);
            var totalCount = filteredList.Count();


            // pagination
            var pagedData = filteredList
             .Skip(((pageNumber - 1) * pageSize))
              .Take(pageSize).ToList();

            var result = pagedData;

            return new PaginatedResponse<T>(result, totalCount, pageNumber, pageSize);
        }

        // Get all/condition, return paginated list of mapped Dtos
        public async Task<PaginatedResponse<TDto>> GetPaginatedResultsAsync<T, TDto>(int pageNumber, int pageSize = int.MaxValue, ISpecification<T> specification = null, CancellationToken cancellationToken = default)
            where T : BaseEntity
            where TDto : IDto
        {
            
            IQueryable<T> query = _context.Set<T>(); // build query
            if (specification != null)
            {
                query = SpecificationEvaluator<T>.GetQuery(_context.Set<T>().AsQueryable(), specification);
            }

            var filteredList = await query.ToListAsync(cancellationToken); // call to db
            var totalCount = filteredList.Count();


            // pagination
            var pagedData = filteredList
             .Skip(((pageNumber - 1) * pageSize))
              .Take(pageSize).ToList();


            var result = _mapper.Map<List<TDto>>(pagedData); // map results to dtos

            return new PaginatedResponse<TDto>(result, totalCount, pageNumber, pageSize);
        }

        #endregion Pagination

        #region Save

        // Save the Changes to Database!
        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        #endregion Save
    }
}
