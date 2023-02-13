using NanoSingular.Application.Common.Marker;

namespace NanoSingular.Application.Common
{
    public interface ICurrentTenantUserService : IScopedService
    {
        string? UserId { get; set; }

        string? TenantId { get; set; }
        public Task<bool> SetTenantUser(string tenant);

    }
}
