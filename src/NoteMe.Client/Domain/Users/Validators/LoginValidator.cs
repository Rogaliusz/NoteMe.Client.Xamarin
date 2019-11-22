using System.Data;
using FluentValidation;
using NoteMe.Client.Framework.Validation;
using NoteMe.Client.ViewModels;

namespace NoteMe.Client.Domain.Users.Validators
{
    public class LoginValidator : AbstractValidator<LoginViewModel>
    {
        public LoginValidator()
        {
            RuleFor(x => x.Email)
                .EmailAddress().WithMessage(ValidationCodes.IsNotValid)
                .NotEmpty().WithMessage(ValidationCodes.MustBeNotEmpty);

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage(ValidationCodes.MustBeNotEmpty);
        }
    }
}