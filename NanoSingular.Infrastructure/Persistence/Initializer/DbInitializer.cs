using MathNet.Numerics.RootFinding;
using Microsoft.AspNetCore.Identity;
using NanoSingular.Domain.Entities;
using NanoSingular.Infrastructure.Identity;
using NanoSingular.Infrastructure.Persistence.Contexts;

namespace NanoSingular.Infrastructure.Persistence.Initializer
{
    public static class DbInitializer
    {
        public static void SeedTenantAdminAndRoles(ApplicationDbContext context)
        {
            ArgumentNullException.ThrowIfNull(context, nameof(context));
            var rootTenant = context.Tenants.FirstOrDefault(x => x.Id == "root"); // if no root tenant is found
            if (rootTenant != null) return;


            //Tenant
            var tenant = new Tenant
            {
                Id = "root",
                Name = "Root",
                IsActive = true,
                CreatedOn = DateTime.UtcNow,
            };
            context.Tenants.Add(tenant);


            //User
            var user = new ApplicationUser
            {
                Id = "55555555-5555-5555-5555-555555555555",
                Email = "admin@email.com",
                NormalizedEmail = "ADMIN@EMAIL.COM",
                UserName = "admin@email.com.root",
                FirstName = "Default",
                LastName = "Admin",
                NormalizedUserName = "ADMIN@EMAIL.COM.ROOT",
                PhoneNumber = null,
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                SecurityStamp = Guid.NewGuid().ToString("D"),
                TenantId = "root",
                DarkModeDefault = true,
                PageSizeDefault = 10
            };

            var password = new PasswordHasher<ApplicationUser>();
            var hashed = password.HashPassword(user, "Password123!");
            user.PasswordHash = hashed;

            context.Users.Add(user);

            //Roles    
            var roles = new List<IdentityRole>();
            roles.Add(new IdentityRole() { Id = "1", Name = "root", ConcurrencyStamp = Guid.NewGuid().ToString("D"), NormalizedName = "ROOT" });
            roles.Add(new IdentityRole() { Id = "2", Name = "admin", ConcurrencyStamp = Guid.NewGuid().ToString("D"), NormalizedName = "ADMIN" });
            roles.Add(new IdentityRole() { Id = "3", Name = "editor", ConcurrencyStamp = Guid.NewGuid().ToString("D"), NormalizedName = "EDITOR" });
            roles.Add(new IdentityRole() { Id = "4", Name = "basic", ConcurrencyStamp = Guid.NewGuid().ToString("D"), NormalizedName = "BASIC" });
            context.Roles.AddRange(roles);


            //User Roles
            var rootAdmin = new IdentityUserRole<string>() { RoleId = "1", UserId = "55555555-5555-5555-5555-555555555555" };
            context.UserRoles.Add(rootAdmin);


            context.SaveChanges();
        }
    }
}