using NanoSingular.Application.Common.Marker;
using NanoSingular.Application.Common.Wrapper;
using NanoSingular.Infrastructure.Auth.DTOs;

namespace NanoSingular.Infrastructure.Auth
{
    public interface ITokenService : ITransientService
    {
        Task<Response<TokenResponse>> GetTokenAsync(TokenRequest request);
    }
}
