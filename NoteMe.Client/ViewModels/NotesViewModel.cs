using System.Collections.ObjectModel;
using System.Threading.Tasks;
using NoteMe.Common.Domain.Notes.Dto;
using NoteMe.Common.Domain.Notes.Queries;
using NoteMe.Common.Domain.Pagination;

namespace NoteMe.Client.ViewModels
{
    public class NotesViewModel : BaseViewModel
    {
        public ObservableCollection<NoteDto> Notes { get; set; } = new ObservableCollection<NoteDto>();
        
        protected NotesViewModel(IViewModelFacade viewModelFacade) : base(viewModelFacade)
        {
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