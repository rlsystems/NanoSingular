using NanoSingular.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace NanoSingular.Infrastructure.Identity
{
    public class ApplicationUser : IdentityUser, IAuditableEntity, ISoftDelete
    {
        
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string TenantId { get; set; }

        public bool IsActive { get; set; } = true;
        public string ImageUrl { get; set; }
        public int PageSizeDefault { get; set; }
        public bool DarkModeDefault { get; set; }

        [NotMapped]
        public string RoleId { get; set; }


        //-- Auditable / Soft Delete Fields --//

        public Guid CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid? LastModifiedBy { get; set; }
        public DateTime? LastModifiedOn { get; set; }
        public Guid? DeletedBy { get; set; }
        public DateTime? DeletedOn { get; set; }  
        public bool IsDeleted { get; set; }
    }
}
