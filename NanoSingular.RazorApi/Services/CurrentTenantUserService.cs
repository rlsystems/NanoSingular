using NanoSingular.Application.Common;
using System.Security.Claims;

namespace NanoSingular.RazorApi.Services
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
            //UserId = _httpContextAccessor?.HttpContext?.User?.FindFirstValue("uid"); // -- this was from token
            //TenantId = _httpContextAccessor?.HttpContext?.User?.FindFirstValue("tenantid"); // -- this was from token
            TenantId = tenant;
            UserId =  _httpContextAccessor?.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier); //this is equivalent of get UserID (guid) // will be null on login
            return true;
        }



        public string? UserId { get; set; }
        public string? TenantId { get; set; }
    }
}
