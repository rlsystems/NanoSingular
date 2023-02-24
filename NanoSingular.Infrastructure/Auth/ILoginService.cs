using NanoSingular.Application.Common.Marker;
using NanoSingular.Application.Common.Wrapper;
using NanoSingular.Infrastructure.Auth.DTOs;

namespace NanoSingular.Infrastructure.Auth
{
    public interface ILoginService : ITransientService
    {
        Task<Response> LoginAsync(TokenRequest request);
    }
}
