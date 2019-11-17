using NoteMe.Client.Framework;
using NoteMe.Client.Framework.Cqrs;
using NoteMe.Client.Framework.Mappers;

namespace NoteMe.Client.ViewModels
{
    public interface IViewModelFacade
    {
        INavigationService NavigationService { get; }
        ICommandDispatcher CommandDispatcher { get; }
        INoteMeClientMapper Mapper { get; }
    }
    
    public class ViewModelFacade : IViewModelFacade
    {
        public ViewModelFacade(INavigationService navigationService, ICommandDispatcher commandDispatcher, INoteMeClientMapper mapper)
        {
            NavigationService = navigationService;
            CommandDispatcher = commandDispatcher;
            Mapper = mapper;
        }

        public INavigationService NavigationService { get; }
        public ICommandDispatcher CommandDispatcher { get; }
        public INoteMeClientMapper Mapper { get; }
        
    }
}