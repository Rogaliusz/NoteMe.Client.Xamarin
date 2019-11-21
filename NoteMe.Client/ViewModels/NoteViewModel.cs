using System.Collections.ObjectModel;
using System.Threading.Tasks;
using N.Publisher;
using NoteMe.Client.Domain.Notes.Messages;
using NoteMe.Common.Domain.Notes.Dto;
using NoteMe.Common.Domain.Notes.Queries;
using NoteMe.Common.Domain.Pagination;
using Xamarin.Essentials;

namespace NoteMe.Client.ViewModels
{
    public class NoteViewModel : ViewModelBase
    {
        private readonly NSubscription _newNotesSubscription;
        public ObservableCollection<NoteDto> Notes { get; set; } = new ObservableCollection<NoteDto>();
        
        protected NoteViewModel(IViewModelFacade viewModelFacade) : base(viewModelFacade)
        {
            _newNotesSubscription = NPublisher.SubscribeIt<NewNotesMessage>(message =>
            {
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    foreach (var note in message.Notes)
                    {
                        Notes.Add(note);
                    }
                });
            });
        }

        public override async Task InitializeAsync(object parameter = null)
        {
            await base.InitializeAsync(parameter);
            
            var query = new GetNotesQuery();
            var notes = await DispatchQueryAsync<GetNotesQuery, PaginationDto<NoteDto>>(query);

            foreach (var note in notes.Data)
            {
                Notes.Add(note);
            }
        }
    }
}