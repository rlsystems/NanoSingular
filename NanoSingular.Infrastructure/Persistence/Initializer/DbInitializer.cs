using Microsoft.AspNetCore.Identity;
using NanoSingular.Infrastructure.Identity;

namespace NanoSingular.Infrastructure.Persistence.Initializer
{
    public static class DbInitializer
    {
        public static ApplicationUser SeedUsers()
        {
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
                DarkModeDefault = true,
                PageSizeDefault = 10
            };

            var password = new PasswordHasher<ApplicationUser>();
            user.PasswordHash = password.HashPassword(user, "Password123!");

            return user;
        }

        public static List<IdentityRole> SeedRoles()
        {
            return new List<IdentityRole>
            {
                new IdentityRole() { Id = "1", Name = "root", ConcurrencyStamp = Guid.NewGuid().ToString("D"), NormalizedName = "ROOT" },
                new IdentityRole() { Id = "2", Name = "admin", ConcurrencyStamp = Guid.NewGuid().ToString("D"), NormalizedName = "ADMIN" },
                new IdentityRole() { Id = "3", Name = "editor", ConcurrencyStamp = Guid.NewGuid().ToString("D"), NormalizedName = "EDITOR" },
                new IdentityRole() { Id = "4", Name = "basic", ConcurrencyStamp = Guid.NewGuid().ToString("D"), NormalizedName = "BASIC" }
            };
        }

        public static IdentityUserRole<string> SeedUserRoles()
        {
            return new IdentityUserRole<string>() { RoleId = "1", UserId = "55555555-5555-5555-5555-555555555555" };
        }
    }
}