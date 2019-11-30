using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using NoteMe.Client.Domain.Notes;
using NoteMe.Client.Domain.Notes.Commands;
using NoteMe.Client.Domain.Notes.Handlers;
using NoteMe.Client.Framework.Platform;
using NoteMe.Client.ViewModels.Forms;
using NoteMe.Common.DataTypes.Enums;
using NoteMe.Common.Domain.Notes.Commands;
using Plugin.FilePicker;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace NoteMe.Client.ViewModels
{
    public class NoteCreateViewModel : ViewModelBase, INoteForm
    {
        private readonly IAttachmentHandler _attachmentHandler;

        private string _name;
        private string _tags;
        private string _content;
        private Attachment _currentAttachment;

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

        protected NoteCreateViewModel(
            IAttachmentHandler attachmentHandler,
            IViewModelFacade viewModelFacade) : base(viewModelFacade)
        {
            _attachmentHandler = attachmentHandler;
            
            SaveCommand = new Command(async () => await CreateNoteAsync(), Validate);
            UploadCommand = new Command(async () => await AddAttachmentAsync());
            OpenAttachmentCommand = new Command(async () => await _attachmentHandler.OpenAsync(CurrentAttachment ?? Attachments.FirstOrDefault()));
        }

        private async Task AddAttachmentAsync()
            => CurrentAttachment = await _attachmentHandler.TakePhotoAsync(Attachments) ?? CurrentAttachment;

        protected override void IsValidChanged()
        {
            SaveCommand.ChangeCanExecute();
        }

        private async Task CreateNoteAsync()
        {
            var command = MapTo<CreateNoteInSqliteCommand>(this);

            await DispatchCommandAsync(command);
            await NavigateTo("//notes");

            Name = string.Empty;
            Tags = string.Empty;
            Content = string.Empty;

            Attachments.Clear();
        }
        
        private void CurrentAttachmentChangedHandler()
        {
            Console.WriteLine(CurrentAttachment);
        }
    }
}