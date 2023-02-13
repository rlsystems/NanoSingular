using NanoSingular.Application.Common.Marker;
using FluentValidation;

namespace NanoSingular.Infrastructure.Identity.DTOs
{
    public class RegisterUserRequest : IDto
    {

        public string FirstName { get; set; }
      
        public string LastName { get; set; }
  
        public string Email { get; set; }

        public string Password { get; set; }

        public string PhoneNumber { get; set; }

        public string RoleId { get; set; }
    }

    public class RegisterUserValidator : AbstractValidator<RegisterUserRequest>
    {
        public RegisterUserValidator()
        {
            RuleFor(x => x.Email).NotEmpty().EmailAddress();
            RuleFor(x => x.FirstName).NotEmpty();
            RuleFor(x => x.LastName).NotEmpty();
            RuleFor(x => x.Password).NotEmpty().WithMessage("Please enter the password");


            List<string> conditions = new List<string>() { "admin", "editor", "basic" };
            RuleFor(x => x.RoleId).Must(x => conditions.Contains(x))
                    .WithMessage("Please only use: " + String.Join(", ", conditions));


        }
    }
}
