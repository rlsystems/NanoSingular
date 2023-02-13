using NanoSingular.Application.Common.Marker;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NanoSingular.Infrastructure.Identity.DTOs
{
    public class UpdatePreferencesRequest : IDto
    {
        public bool DarkModeDefault { get; set; }
        public int PageSizeDefault { get; set; }
      
    }

    public class UpdatePreferencesValidator : AbstractValidator<UpdatePreferencesRequest>
    {
        public UpdatePreferencesValidator()
        {
            RuleFor(x => x.PageSizeDefault).Must(x => x.Equals(5) || x.Equals(10) || x.Equals(25) || x.Equals(50))
                .WithMessage("Use only 5, 10, 25, or 50");
        }
    }
}
