using FluentValidation;
using NoteMe.Client.Framework.Validation;
using NoteMe.Client.ViewModels;
using NoteMe.Client.ViewModels.Forms;

namespace NoteMe.Client.Domain.Notes.Validators
{
    public class NoteFormValidator : AbstractValidator<INoteForm>
    {
        public NoteFormValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage(ValidationCodes.MustBeNotEmpty);
            
            RuleFor(x => x.Content)
                .NotEmpty().WithMessage(ValidationCodes.MustBeNotEmpty);
        }
    }
}