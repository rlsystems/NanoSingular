using NanoSingular.Application.Common.Marker;

namespace NanoSingular.Infrastructure.Multitenancy.DTOs
{
    public class TenantDTO : IDto
    {
        public string Id { get; set; }
        public string Key { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedOn { get; set; }


    }
}
