using System.Security.Claims;
using NanoSingular.Application.Common;

// This is a scoped service that does not persist data, it fits better in the WebApi project vs the Infrastructure project
// --This class uses a seperate DB Context to avoid a circular logic error when it looks up the tenant
// --Query filters in ApplicationDbContext depend on having a 'TenantId' present in CurrentTenantUserService

namespace NanoSingular.WebApi.Services
{
    //public class CurrentTenantUserService : ICurrentTenantUserService
    //{
    //    private readonly IHttpContextAccessor _httpContextAccessor;

    //    public CurrentTenantUserService(IHttpContextAccessor httpContextAccessor)
    //    {
    //        _httpContextAccessor = httpContextAccessor;
    //    }

    //    public string? UserId => GetUser();

    //    // the custom tenant claim will be added at sign in
    //    public string? TenantId => GetTenant();

    //    private string? GetUser()
    //    {
    //        var user = _httpContextAccessor?.HttpContext?.User;

    //        return user?.FindFirstValue("uid");
    //    }

    //    private string? GetTenant()
    //    {
    //        var context = _httpContextAccessor?.HttpContext;

    //        if (context == null)
    //        {
    //            return null;
    //        }

    //        var tenantFromAuth = context.User.FindFirstValue("tenant");

    //        // if we have a tenant in the claim return it
    //        if (!string.IsNullOrEmpty(tenantFromAuth))
    //        {
    //            return tenantFromAuth;
    //        }

    //        // try to get the claim from the header
    //        context.Request.Headers.TryGetValue("tenant", out var tenantFromHeader); // Tenant Id from incoming request header

    //        //return if in the header otherwise default to root
    //        return string.IsNullOrEmpty(tenantFromHeader) ? "root" : tenantFromHeader; // Fallback (Needed to load the react client initially)
    //    }
    //}
}