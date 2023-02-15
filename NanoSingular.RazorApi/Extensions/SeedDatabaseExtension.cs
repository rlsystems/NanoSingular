using NanoSingular.Infrastructure.Persistence.Contexts;
using NanoSingular.Infrastructure.Persistence.Initializer;

namespace NanoSingular.RazorApi.Extensions
{
    public static class SeedDatabaseExtension
    {
        public static IApplicationBuilder SeedDatabase(this IApplicationBuilder app)
        {
            using var scope = app.ApplicationServices.CreateScope();
            var services = scope.ServiceProvider;
            try
            {
                var context = services.GetRequiredService<ApplicationDbContext>();
                context.Database.EnsureCreated();
                DbInitializer.SeedTenantAdminAndRoles(context); // seed database with root tenant, root admin, and default roles
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return app;
        }
    }
}