using FluentValidation;
using NoteMe.Client.Framework.Validation;
using NoteMe.Client.ViewModels;

namespace NoteMe.Client.Domain.Users.Validators
{
    public class RegisterValidator : AbstractValidator<RegisterViewModel>
    {
        public RegisterValidator()
        {
            RuleFor(x => x.Email)
                .EmailAddress().WithMessage(ValidationCodes.IsNotValid)
                .NotEmpty().WithMessage(ValidationCodes.MustBeNotEmpty);

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage(ValidationCodes.MustBeNotEmpty);

            RuleFor(x => x.ConfirmPassword)
                .NotEmpty().WithMessage(ValidationCodes.MustBeNotEmpty)
                .Equal(x => x.Password).WithMessage(ValidationCodes.MustBeEqual);
        }
    }
}