using FluentValidation;
using ProniaOnion202.Application.DTOs.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProniaOnion202.Application.Validators
{
    public class RegisterDtoValidator : AbstractValidator<RegisterDto>
    {
        public RegisterDtoValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress()
                .MaximumLength(256);

            RuleFor(x => x.Password)
                .NotEmpty()
                .MinimumLength(8)
                .MaximumLength(100);
            RuleFor(x => x.UserName)
                .NotEmpty()
                .MaximumLength(256)
                .MinimumLength(4);
            RuleFor(x => x.Name)
                .NotEmpty()
                .MaximumLength(50)
                .MinimumLength(3);
            RuleFor(x => x.Surname)
               .NotEmpty()
               .MaximumLength(50)
               .MinimumLength(3);
            RuleFor(x => x)
                .Must(x => x.ConfirmPassword == x.Password);

        }
    }
}