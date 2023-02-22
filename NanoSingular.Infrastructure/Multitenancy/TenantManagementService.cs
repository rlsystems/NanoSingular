using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NanoSingular.Application.Common.Wrapper;
using NanoSingular.Domain.Entities;
using NanoSingular.Infrastructure.Identity;
using NanoSingular.Infrastructure.Multitenancy.DTOs;
using NanoSingular.Infrastructure.Persistence.Contexts;


namespace NanoSingular.Infrastructure.Multitenancy
{
    public class TenantManagementService : ITenantManagementService
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        // Use ApplicationDbContext directly, tenant is not derived from Base Entity (doesnt use GUID for Key)
        // There is no delete method, instead set active to true or false

        public TenantManagementService(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<Response<List<Tenant>>> GetAllTenants()
        {

            var list = await _context.Tenants.ToListAsync(); // Returns a non-paginated list

            return Response<List<Tenant>>.Success(list);
        }

        public async Task<Response<Tenant>> SaveTenant(CreateTenantRequest request)
        {
            // Check if tenant exists 
            var exists = await _context.Tenants.AnyAsync(x => x.Id == request.Id);
            if (exists)
                return Response<Tenant>.Fail("Tenant already exists");

            // Save the Tenant
            Tenant tenant = new Tenant();
            tenant.Id = request.Id;
            tenant.Name = request.Name;
            tenant.CreatedOn = DateTime.UtcNow;
            tenant.IsActive = true;

            try
            {
                await _context.Tenants.AddAsync(tenant);

                var tenantId = tenant.Id; // Set the new Tenant Id

                var user = new ApplicationUser
                {
                    UserName = request.AdminEmail + "." + tenant.Id, // Username must be unique: Email + TenantId
                    FirstName = "Default",
                    LastName = "Admin",
                    Email = request.AdminEmail,
                    EmailConfirmed = true,
                    DarkModeDefault = true,
                    PageSizeDefault = 10,
                    TenantId = tenantId
                };

                var result = await _userManager.CreateAsync(user, request.Password); // Create a default admin user upon Tenant creation
                if (result.Succeeded) // Success - Add to Admin role
                {
                    await _userManager.AddToRoleAsync(user, "admin");
                    await _context.SaveChangesAsync();
                    return Response<Tenant>.Success(tenant); // return the newly created object if success
                }
                else // Fail - return error messages
                {
                    var messages = new List<string>();
                    foreach (var error in result.Errors)
                    {
                        messages.Add(error.Description);
                    }
                    return Response<Tenant>.Fail(messages);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<Response<Tenant>> UpdateTenant(UpdateTenantRequest request)
        {
            // Get by Id
            var tenant = await _context.Tenants.Where(x => x.Id == request.Id).FirstOrDefaultAsync();


            // Check if exists
            if (tenant == null)
                return Response<Tenant>.Fail("Not Found");

            if (tenant.Id == "root")
                return Response<Tenant>.Fail("Cannot edit root tenant");

            // Update
            tenant.IsActive = request.IsActive;
            tenant.Name = request.Name;

            try
            {
                await _context.SaveChangesAsync();

                return Response<Tenant>.Success(tenant); // return the updated object if success

            }
            catch (Exception ex)
            {
                return Response<Tenant>.Fail(ex.Message);
            }
        }
    }

}
