using FluentValidation;
using NanoSingular.Application.Common.Marker;


namespace NanoSingular.Infrastructure.Multitenancy.DTOs
{
    public class UpdateTenantRequest : IDto
    {
        public bool IsActive { get; set; }
        public string Id { get; set; }
        public string Name { get; set; }
    }

    public class UpdateTenantValidator : AbstractValidator<UpdateTenantRequest>
    {
        public UpdateTenantValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
            RuleFor(x => x.Name).NotEmpty();
        }
    }
}
