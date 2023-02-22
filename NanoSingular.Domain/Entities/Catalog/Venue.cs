
// Venue (sample business entity)
namespace NanoSingular.Domain.Entities
{
    public class Venue: AuditableEntity
    {
        public string Name { get; set; } 
        public string Description { get; set; }

    }
}
