using NoteMe.Client.Framework;
using NoteMe.Client.Framework.Cqrs;
using NoteMe.Client.Framework.Extensions;
using NoteMe.Client.Framework.Mappers;
using NoteMe.Client.Framework.Ui;
using NoteMe.Client.Framework.Validation;

namespace NoteMe.Client.ViewModels
{
    public interface IViewModelFacade
    {
        ITranslationService TranslationService { get; }
        IValidationDispatcher ValidationDispatcher { get; }
        INavigationService NavigationService { get; }
        ICommandDispatcher CommandDispatcher { get; }
        INoteMeClientMapper Mapper { get; }
        IQueryDispatcher QueryDispatcher { get; }
        IDialogService DialogService { get; }
        
    }
    
    public class ViewModelFacade : IViewModelFacade
    {
        public ViewModelFacade(ITranslationService translationService, IValidationDispatcher validationDispatcher, INavigationService navigationService, ICommandDispatcher commandDispatcher, INoteMeClientMapper mapper, IQueryDispatcher queryDispatcher, IDialogService dialogService)
        {
            TranslationService = translationService;
            ValidationDispatcher = validationDispatcher;
            NavigationService = navigationService;
            CommandDispatcher = commandDispatcher;
            Mapper = mapper;
            QueryDispatcher = queryDispatcher;
            DialogService = dialogService;
        }

        public ITranslationService TranslationService { get; }
        public IValidationDispatcher ValidationDispatcher { get; }
        public INavigationService NavigationService { get; }
        public ICommandDispatcher CommandDispatcher { get; }
        public INoteMeClientMapper Mapper { get; }
        public IQueryDispatcher QueryDispatcher { get; }
        public IDialogService DialogService { get; }
    }
}