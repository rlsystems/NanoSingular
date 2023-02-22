using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NanoSingular.Application.Common;
using NanoSingular.Domain.Entities;
using NanoSingular.Infrastructure.Identity;
using NanoSingular.Infrastructure.Persistence.Extensions;
using NanoSingular.Infrastructure.Persistence.Initializer;

//---------------------------------- CLI COMMANDS --------------------------------------------------

// Set default project to NanoSingular.Infrastructure in Package Manager Console
// When scaffolding database migrations, you must specify which context (ApplicationDbContext), use the following command:

// add-migration -Context ApplicationDbContext -o Persistence/Migrations MigrationName
// update-database -Context ApplicationDbContext

//--------------------------------------------------------------------------------------------------

namespace NanoSingular.Infrastructure.Persistence.Contexts
{
    public class ApplicationRoles : IdentityRole
    {
    }

    // This is the main context class
    // -- migrations are run using this context
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        private readonly ICurrentTenantUserService _currentTenantUserService;
        public string CurrentTenantId { get; set; }
        public string CurrentUserId { get; set; }

        public ApplicationDbContext(ICurrentTenantUserService currentTenantUserService, DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            _currentTenantUserService = currentTenantUserService;
            CurrentTenantId = _currentTenantUserService.TenantId;
            CurrentUserId = _currentTenantUserService.UserId;
        }
        public DbSet<Tenant> Tenants { get; set; }

        public DbSet<Venue> Venues { get; set; }

        // Apply global query filters, rename tables, and run seeders
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);


            // rename identity tables
            builder.ApplyIdentityConfiguration();

            builder.Entity<ApplicationUser>().HasQueryFilter(a => a.TenantId == CurrentTenantId); // multitenancy query filter for Application User
            builder.AppendGlobalQueryFilter<IMustHaveTenant>(b => b.TenantId == CurrentTenantId); // multitenancy query filter for all other entities, uses extension method in ModelBuilderExtentions
            builder.AppendGlobalQueryFilter<ISoftDelete>(s => !s.IsDeleted); // filter out deleted entities (soft delete)

            // seed static data
            builder.SeedStaticData();
        }

        // Handle audit fields (createdOn, createdBy, modifiedBy, modifiedOn) and handle soft delete on save changes
        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
 

            // it has to be IAuditableEntity not AuditableEntity
            foreach (var entry in ChangeTracker.Entries<IAuditableEntity>().ToList()) // Auditable fields / soft delete on tables with IAuditableEntity
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedBy = CurrentUserId != null ? Guid.Parse(CurrentUserId) : Guid.Empty;
                        entry.Entity.CreatedOn = DateTime.UtcNow;
                        break;

                    case EntityState.Modified:
                        entry.Entity.LastModifiedOn = DateTime.UtcNow;
                        entry.Entity.LastModifiedBy = CurrentUserId != null ? Guid.Parse(CurrentUserId) : Guid.Empty;
                        break;

                    case EntityState.Deleted:
                        if (entry.Entity is ISoftDelete softDelete) // intercept delete requests, forward as modified on tables with ISoftDelete
                        {
                            softDelete.DeletedBy = CurrentUserId != null ? Guid.Parse(CurrentUserId) : Guid.Empty;
                            softDelete.DeletedOn = DateTime.UtcNow;
                            softDelete.IsDeleted = true;
                            entry.State = EntityState.Modified;
                        }

                        break;
                }
            }

            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}