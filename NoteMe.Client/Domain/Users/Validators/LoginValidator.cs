using System.Data;
using FluentValidation;
using NoteMe.Client.ViewModels;

namespace NoteMe.Client.Domain.Users.Validators
{
    public class LoginValidator : AbstractValidator<LoginViewModel>
    {
        public LoginValidator()
        {
            RuleFor(x => x.Email)
                .EmailAddress()
                .NotEmpty();

            RuleFor(x => x.Password)
                .NotEmpty();
        }
    }
}