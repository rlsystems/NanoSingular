using NanoSingular.Application.Common.Marker;
using NanoSingular.Application.Common.Specification;
using NanoSingular.Application.Common.Wrapper;
using NanoSingular.Domain.Entities;


namespace NanoSingular.Application.Common
{
    public interface IRepositoryAsync : ITransientService
    {


        Task<IEnumerable<T>> GetListAsync<T>(ISpecification<T> specification = null, CancellationToken cancellationToken = default)
        where T : BaseEntity;

        Task<IEnumerable<TDto>> GetListAsync<T, TDto>(ISpecification<T> specification = null, CancellationToken cancellationToken = default)
            where T : BaseEntity
            where TDto : IDto;

        Task<T> GetByIdAsync<T>(Guid id, ISpecification<T> specification = null, CancellationToken cancellationToken = default)
        where T : BaseEntity;

        Task<TDto> GetByIdAsync<T, TDto>(Guid id, ISpecification<T> specification = null, CancellationToken cancellationToken = default)
        where T : BaseEntity
        where TDto : IDto;

        Task<bool> ExistsAsync<T>(ISpecification<T> specification = null, CancellationToken cancellationToken = default)
        where T : BaseEntity;

        Task<T> CreateAsync<T>(T entity)
        where T : BaseEntity;

        Task<IList<Guid>> CreateRangeAsync<T>(IEnumerable<T> entity)
        where T : BaseEntity;

        Task<T> UpdateAsync<T>(T entity)
        where T : BaseEntity;

        Task RemoveAsync<T>(T entity)
        where T : BaseEntity;

        Task<T> RemoveByIdAsync<T>(Guid entityId)
        where T : BaseEntity;

        Task<PaginatedResponse<T>> GetPaginatedResultsAsync<T>(int pageNumber, int pageSize = int.MaxValue, ISpecification<T> specification = null, CancellationToken cancellationToken = default)
        where T : BaseEntity;

        Task<PaginatedResponse<TDto>> GetPaginatedResultsAsync<T, TDto>(int pageNumber, int pageSize = int.MaxValue, ISpecification<T> specification = null, CancellationToken cancellationToken = default)
        where T : BaseEntity
        where TDto : IDto;


        Task<int> SaveChangesAsync();

    }
}
