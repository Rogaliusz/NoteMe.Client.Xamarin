using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using NoteMe.Client.Domain.Notes;
using NoteMe.Client.Domain.Notes.Commands;
using NoteMe.Client.Domain.Notes.Handlers;
using NoteMe.Client.ViewModels.Forms;
using NoteMe.Common.DataTypes.Domain.Notes.Queries;
using Xamarin.Forms;

namespace NoteMe.Client.ViewModels
{
    public class NoteUpdateViewModel : ViewModelBase, INoteForm
    {
        private readonly IAddAttachmentHandler _addAttachmentHandler;
        private string _name;
        private string _tags;
        private string _content;
        private Attachment _currentAttachment;
        private Note _note;

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
        
        public Attachment CurrentAttachment
        {
            get => _currentAttachment;
            set
            {
                SetProperty(ref _currentAttachment, value);
                CurrentAttachmentChangedHandler();
            }
        }

        public ObservableCollection<Attachment> Attachments { get; set; } = new ObservableCollection<Attachment>();
        
        public ICommand SaveCommand { get; }
        public ICommand UploadCommand { get; }
        
        protected NoteUpdateViewModel(
            IAddAttachmentHandler addAttachmentHandler,
            IViewModelFacade viewModelFacade) : base(viewModelFacade)
        {
            _addAttachmentHandler = addAttachmentHandler;
            
            SaveCommand = new Command(async () => await UpdateNoteAsync(), Validate);
            UploadCommand = new Command(async () => await AddAttachmentAsync());
        }

        private void CurrentAttachmentChangedHandler()
        {

        }
        
        private async Task UpdateNoteAsync()
        {
            MapTo(this, _note);

            await DispatchCommandAsync(new UpdateNoteSqliteCommand(_note));
        }

        private Task AddAttachmentAsync()
            => _addAttachmentHandler.AddAsync(Attachments);

        public override async Task InitializeAsync(object parameter = null)
        {
            await base.InitializeAsync(parameter);
            
            var query = new GetNoteQuery
            {
                Id = Guid.Parse(parameter.ToString())
            };

            _note = await DispatchQueryAsync<GetNoteQuery, Note>(query);

            Name = _note.Name;
            Content = _note.Content;

            foreach (var attachment in _note.Attachments)
            {
                Attachments.Add(attachment);
            }
        }
    }
}