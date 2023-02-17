using NanoSingular.Application.Common.Marker;

namespace NanoSingular.Application.Common
{
    public interface ICurrentTenantUserService : IScopedService
    {
        string? UserId { get; }

        string? TenantId { get; }
    }
}