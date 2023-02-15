using System.Security.Claims;
using NanoSingular.Application.Common;

namespace NanoSingular.RazorApi.Middleware
{
    public class UserResolver
    {
        private readonly RequestDelegate _next;

        public UserResolver(RequestDelegate next)
        {
            _next = next;
        }

        // Get User Id from incoming requests
        public async Task InvokeAsync(HttpContext context, ICurrentTenantUserService newCurrentTenantService)
        {
            var tenantFromAuth = context.User?.FindFirstValue("tenant");
            if (!string.IsNullOrEmpty(tenantFromAuth))
            {
                await newCurrentTenantService.SetTenantUser(tenantFromAuth); // This will set the tenant Id and the user Id (from the token)
            }
            else
            {
                context.Request.Headers.TryGetValue("tenant", out var tenantFromHeader); // Tenant Id from incoming request header
                if (string.IsNullOrEmpty(tenantFromHeader) == false)
                {
                    await newCurrentTenantService.SetTenantUser(tenantFromHeader);
                }
                else
                {
                    await newCurrentTenantService.SetTenantUser("root"); // Fallback (Needed to load the react client initially)
                }
            }

            await _next(context);
        }
    }
}