using NanoSingular.Application.Common;
using System.Security.Claims;

namespace NanoSingular.WebApi.Middleware
{
    public class UserResolver
    {
        private readonly RequestDelegate _next;
        public UserResolver(RequestDelegate next)
        {
            _next = next;
        }

        // Get User Id from incoming requests 
        public async Task InvokeAsync(HttpContext context, ICurrentTenantUserService newCurrentTenantService, IWebHostEnvironment env)
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

        // Once the tenant Id is set, queries will only see users belonging to this tenant - this is made possible with global query filters in ApplicationDbContext 
        // Next the username / password from the request body will be checked and the user will be issued a JWT token if valid     


    }
}
