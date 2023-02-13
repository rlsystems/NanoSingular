using NanoSingular.Application.Common.Marker;

namespace NanoSingular.Infrastructure.Auth.DTOs
{
    public class TokenResponse : IDto
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }
        public DateTime RefreshTokenExpiryTime { get; set; }

    }
}
