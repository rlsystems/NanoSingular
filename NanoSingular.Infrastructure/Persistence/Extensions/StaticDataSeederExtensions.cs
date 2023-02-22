using Microsoft.EntityFrameworkCore;

namespace NanoSingular.Infrastructure.Persistence.Extensions
{
    public static class StaticDataSeederExtensions
    {
        public static void SeedStaticData(this ModelBuilder builder)
        {
            // For Example

            //builder.Entity<VenueType>().HasData(
            //    new VenueType() { Id = 1, Name = "typeA" },
            //    new VenueType() { Id = 2, Name = "typeB" },
            //    new VenueType() { Id = 3, Name = "typeC" }
            //    );
        }
    }
}
