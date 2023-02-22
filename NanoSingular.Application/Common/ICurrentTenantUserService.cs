using NanoSingular.Application.Common.Marker;

namespace NanoSingular.Application.Common
{
    public interface ICurrentTenantUserService : IScopedService
    {
        string? TenantId { get; set; }
        string? UserId { get; set; }
        public Task<bool> SetTenantUser(string tenant);
    }
}