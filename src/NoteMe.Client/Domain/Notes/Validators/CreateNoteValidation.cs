using FluentValidation;
using NoteMe.Client.Framework.Validation;
using NoteMe.Client.ViewModels;

namespace NoteMe.Client.Domain.Notes.Validators
{
    public class CreateNoteValidator : AbstractValidator<CreateNoteViewModel>
    {
        public CreateNoteValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage(ValidationCodes.MustBeNotEmpty);
            
            RuleFor(x => x.Content)
                .NotEmpty().WithMessage(ValidationCodes.MustBeNotEmpty);
        }
    }
}