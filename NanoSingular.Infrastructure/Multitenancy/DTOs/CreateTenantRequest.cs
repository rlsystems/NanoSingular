using FluentValidation;
using NanoSingular.Application.Common.Marker;

namespace NanoSingular.Infrastructure.Multitenancy.DTOs
{
    public class CreateTenantRequest : IDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string AdminEmail { get; set; }
        public string Password { get; set; }

    }

    public class CreateTenantValidator : AbstractValidator<CreateTenantRequest>
    {
        public CreateTenantValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.AdminEmail).NotEmpty().EmailAddress();
        }
    }
}
