using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NanoSingular.Infrastructure.Identity.DTOs
{
    public class ResetPasswordRequest
    {
        public string Email { get; set; }

        public string Password { get; set; }
        public string ConfirmPassword { get; set; }

        public string Token { get; set; }
    }


    public class ResetPasswordRequestValidator : AbstractValidator<ResetPasswordRequest>
    {
        public ResetPasswordRequestValidator()
        {
            RuleFor(x => x.Token).NotEmpty();
            RuleFor(x => x.Email).NotEmpty().EmailAddress();

            RuleFor(p => p.Password)
                .NotEmpty();

            RuleFor(p => p.ConfirmPassword)
                .NotEmpty();

            RuleFor(p => p.ConfirmPassword)
                .Equal(p => p.Password)
                    .WithMessage("Passwords do not match.");
        }
    }
}
