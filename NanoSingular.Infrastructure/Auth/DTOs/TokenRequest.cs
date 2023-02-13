using NanoSingular.Application.Common.Marker;

namespace NanoSingular.Infrastructure.Auth.DTOs
{
    public class TokenRequest : IDto
    {
        //tenant key in header
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
