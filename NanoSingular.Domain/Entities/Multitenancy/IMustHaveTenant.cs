namespace NanoSingular.Domain.Entities
{
    public interface IMustHaveTenant
    {
        public string TenantId { get; set; }
    }
}
