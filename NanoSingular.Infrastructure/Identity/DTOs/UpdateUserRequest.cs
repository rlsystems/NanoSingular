using NanoSingular.Application.Common.Marker;
using FluentValidation;

namespace NanoSingular.Infrastructure.Identity.DTOs
{
    public class UpdateUserRequest : IDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public bool IsActive { get; set; }
        public string RoleId { get; set; }

    }

    public class UpdateUserValidator : AbstractValidator<UpdateUserRequest>
    {
        public UpdateUserValidator()
        {
            RuleFor(x => x.FirstName).NotEmpty();
            RuleFor(x => x.LastName).NotEmpty();
            RuleFor(x => x.Email).NotEmpty().EmailAddress();
            RuleFor(x => x.IsActive).NotNull(); //Null will accept true or false

            List<string> conditions = new List<string>() { "admin", "editor", "basic" };
            RuleFor(x => x.RoleId).Must(x => conditions.Contains(x))
                    .WithMessage("Please only use: " + String.Join(", ", conditions));

        }
    }
}
