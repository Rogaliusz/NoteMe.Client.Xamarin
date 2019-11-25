using System.Collections.ObjectModel;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Windows.Input;
using NoteMe.Common.Domain.Notes.Commands;
using Xamarin.Forms;

namespace NoteMe.Client.ViewModels
{
    public class CreateNoteViewModel : ViewModelBase
    {
        private string _name;
        private string _tags;
        private string _content;

        public string Name
        {
            get => _name;
            set => SetPropertyAndValidate(ref _name, value);
        }
        
        public string Tags
        {
            get => _tags;
            set => SetPropertyAndValidate(ref _tags, value);
        }
        
        public string Content
        {
            get => _content;
            set => SetPropertyAndValidate(ref _content, value);
        }
        
        public ObservableCollection<Attachment> Attachments { get; set; } = new ObservableCollection<Attachment>();
        
        public ICommand CreateCommand { get; }
        public ICommand UploadCommand { get; }
        
        protected CreateNoteViewModel(IViewModelFacade viewModelFacade) : base(viewModelFacade)
        {
            CreateCommand = new Command(async () => await CreateNoteAsync(), Validate);
            UploadCommand = new Command(async () => await AddAttachmentAsync());
        }

        private async Task AddAttachmentAsync()
        {

        }

        protected override void IsValidChanged()
        {
            ((Command)CreateCommand).ChangeCanExecute();
        }

        private async Task CreateNoteAsync()
        {
            var command = MapTo<CreateNoteCommand>(this);

            await DispatchCommandAsync(command);
            await NavigateTo("//notes");
        }
    }
}