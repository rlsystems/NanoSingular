namespace NanoSingular.Domain.Entities
{
    public abstract class TenantBaseEntity : BaseEntity, IMustHaveTenant
    {
        public string TenantId { get; set; }

    }
}
