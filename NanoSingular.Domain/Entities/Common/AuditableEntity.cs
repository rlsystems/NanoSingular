

namespace NanoSingular.Domain.Entities
{

    public abstract class AuditableEntity : BaseEntity, IAuditableEntity, ISoftDelete
    {
        public Guid CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid? LastModifiedBy { get; set; }
        public DateTime? LastModifiedOn { get; set; }
        public Guid? DeletedBy { get; set; }
        public DateTime? DeletedOn { get; set; }
        public bool IsDeleted { get; set; }
 
    }

}
