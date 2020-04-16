using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using N.Publisher;
using NoteMe.Client.Domain.Notes;
using NoteMe.Client.Domain.Notes.Commands;
using NoteMe.Client.Domain.Notes.Handlers;
using NoteMe.Client.Domain.Notes.Messages;
using NoteMe.Client.ViewModels.Forms;
using NoteMe.Common.DataTypes.Domain.Notes.Queries;
using Xamarin.Forms;

namespace NoteMe.Client.ViewModels
{
    public class NoteUpdateViewModel : ViewModelBase, INoteForm
    {
        private readonly IAttachmentHandler _attachmentHandler;
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
        
        public Command SaveCommand { get; }
        public Command UploadCommand { get; }
        public Command OpenAttachmentCommand { get; }

        protected NoteUpdateViewModel(
            IAttachmentHandler attachmentHandler,
            IViewModelFacade viewModelFacade) : base(viewModelFacade)
        {
            _attachmentHandler = attachmentHandler;
            
            SaveCommand = new Command(async () => await UpdateNoteAsync(), Validate);
            UploadCommand = new Command(async () => await AddAttachmentAsync());
            OpenAttachmentCommand = new Command(async () => await _attachmentHandler.OpenAsync(CurrentAttachment ?? Attachments.FirstOrDefault()));
        }

        protected override void IsValidChanged()
        {
            ((Command)SaveCommand).ChangeCanExecute();
            base.IsValidChanged();
        }

        private void CurrentAttachmentChangedHandler()
        {

        }
        
        private async Task UpdateNoteAsync()
        {
            _note.Name = Name;
            _note.Content = Content;
            _note.Attachments = Attachments;
            _note.Tags = Tags;

            await DispatchCommandAsync(new UpdateNoteSqliteCommand(_note));
            
            NPublisher.PublishIt(new NewNotesMessage());
        }

        private async Task AddAttachmentAsync()
            => CurrentAttachment = await _attachmentHandler.TakePhotoAsync(Attachments) ?? CurrentAttachment;

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
            Tags = _note.Tags;

            foreach (var attachment in _note.Attachments)
            {
                Attachments.Add(attachment);
            }

            CurrentAttachment = Attachments.FirstOrDefault();
        }
    }
}