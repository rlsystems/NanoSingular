using NanoSingular.Application.Common.Marker;
using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace NanoSingular.Infrastructure.Identity.DTOs
{
    public class UpdateProfileRequest : IDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public IFormFile ImageFile { get; set; }
        public bool DeleteCurrentImage { get; set; } = false;

    }

    public class UpdateProfileValidator : AbstractValidator<UpdateProfileRequest>
    {
        public UpdateProfileValidator()
        {
            RuleFor(x => x.FirstName).NotEmpty();
            RuleFor(x => x.LastName).NotEmpty();
            RuleFor(x => x.Email).NotEmpty().EmailAddress();

        }
    }
}
