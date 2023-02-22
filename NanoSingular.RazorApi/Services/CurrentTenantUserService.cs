using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using NanoSingular.Application.Common;
using NanoSingular.Infrastructure.Persistence.Contexts;

namespace NanoSingular.RazorApi.Services
{
    public class CurrentTenantUserService : ICurrentTenantUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly TenantDbContext _tenantDbContext;

        public CurrentTenantUserService(TenantDbContext tenantDbContext, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _tenantDbContext = tenantDbContext;

        }
        public async Task<bool> SetTenantUser(string tenant)
        {

            var tenantInfo = await _tenantDbContext.Tenants.Where(x => x.Id == tenant && x.IsActive == true).FirstOrDefaultAsync(); // check if tenant exists
            if (tenantInfo != null)
            {
                TenantId = tenant;
                UserId = _httpContextAccessor?.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value.ToString(); // will be null on login
                return true;
            }
            else
            {
                throw new Exception("Tenant invalid"); // will return 500 error
            }

        }
        public string? TenantId { get; set; }
        public string? UserId { get; set; }
    }
}