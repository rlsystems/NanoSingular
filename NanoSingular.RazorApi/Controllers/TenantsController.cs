using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NanoSingular.Infrastructure.Multitenancy.DTOs;
using NanoSingular.Infrastructure.Multitenancy;

namespace NanoSingular.RazorApi.Controllers
{

    [Authorize(Roles = "root")]
    [Route("api/[controller]")]
    [ApiController]
    public class TenantsController : ControllerBase
    {
        private readonly ITenantManagementService _tenantService;

        public TenantsController(ITenantManagementService tenantService)
        {
            _tenantService = tenantService;
        }


        [AllowAnonymous] // AllowAnonymous to populate tenant dropdown on login view (added for demo)
        [HttpGet] // Get all tenants
        public async Task<IActionResult> GetAllTenants()
        {
            var result = await _tenantService.GetAllTenants();
            return Ok(result.Data);
        }


        [HttpPost] // Create new tenant
        public async Task<IActionResult> SaveTenant(CreateTenantRequest request)
        {
            var result = await _tenantService.SaveTenant(request);
            return Ok(result);
        }


        [HttpPut] // Update active/inactive status
        public async Task<IActionResult> UpdateTenant(UpdateTenantRequest request)
        {
            var result = await _tenantService.UpdateTenant(request);
            return Ok(result);
        }
    }
}
