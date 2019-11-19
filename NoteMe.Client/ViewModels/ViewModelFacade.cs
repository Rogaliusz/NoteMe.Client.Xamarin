using NoteMe.Client.Framework;
using NoteMe.Client.Framework.Cqrs;
using NoteMe.Client.Framework.Mappers;
using NoteMe.Client.Framework.Ui;

namespace NoteMe.Client.ViewModels
{
    public interface IViewModelFacade
    {
        INavigationService NavigationService { get; }
        ICommandDispatcher CommandDispatcher { get; }
        INoteMeClientMapper Mapper { get; }
        IQueryDispatcher QueryDispatcher { get; }
        IDialogService DialogService { get; }
        
    }
    
    public class ViewModelFacade : IViewModelFacade
    {
        public ViewModelFacade(INavigationService navigationService, ICommandDispatcher commandDispatcher, INoteMeClientMapper mapper, IQueryDispatcher queryDispatcher, IDialogService dialogService)
        {
            NavigationService = navigationService;
            CommandDispatcher = commandDispatcher;
            Mapper = mapper;
            QueryDispatcher = queryDispatcher;
            DialogService = dialogService;
        }

        public INavigationService NavigationService { get; }
        public ICommandDispatcher CommandDispatcher { get; }
        public INoteMeClientMapper Mapper { get; }
        public IQueryDispatcher QueryDispatcher { get; }
        public IDialogService DialogService { get; }
    }
}