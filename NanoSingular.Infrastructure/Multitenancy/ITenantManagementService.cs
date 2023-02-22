using NanoSingular.Application.Common.Marker;
using NanoSingular.Application.Common.Wrapper;
using NanoSingular.Domain.Entities;
using NanoSingular.Infrastructure.Multitenancy.DTOs;

namespace NanoSingular.Infrastructure.Multitenancy
{
    public interface ITenantManagementService : ITransientService
    {
        Task<Response<List<Tenant>>> GetAllTenants();
        Task<Response<Tenant>> SaveTenant(CreateTenantRequest request);
        Task<Response<Tenant>> UpdateTenant(UpdateTenantRequest request);


    }
}
