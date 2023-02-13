using NanoSingular.Application.Common;
using NanoSingular.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

// This is a scoped service that does not persist data, it fits better in the WebApi project vs the Infrastructure project
// --This class uses a seperate DB Context to avoid a circular logic error when it looks up the tenant
// --Query filters in ApplicationDbContext depend on having a 'TenantId' present in CurrentTenantUserService

namespace NanoSingular.WebApi.Services
{
    public class CurrentTenantUserService : ICurrentTenantUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentTenantUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<bool> SetTenantUser(string tenant)
        {
            //var tenantInfo = await _tenantDbContext.Tenants.Where(x => x.Id == tenant && x.IsActive == true).FirstOrDefaultAsync(); // check if tenant exists

            TenantId = tenant;
            UserId = _httpContextAccessor?.HttpContext?.User?.FindFirstValue("uid"); // will be null on login
            return true;
        }


        public string? UserId { get; set; }
        public string? TenantId { get; set; }
    }
}
