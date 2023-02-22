using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace NanoSingular.Domain.Entities
{
    public class Tenant
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string Id { get; set; } // for simplicity, using magic-strings for tenant ids
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedOn { get; set; }

    }
}
