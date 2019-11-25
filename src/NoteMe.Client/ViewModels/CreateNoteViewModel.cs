using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using NoteMe.Client.Domain.Notes;
using NoteMe.Client.Domain.Notes.Commands;
using NoteMe.Common.DataTypes.Enums;
using NoteMe.Common.Domain.Notes.Commands;
using Plugin.FilePicker;
using Xamarin.Forms;

namespace NoteMe.Client.ViewModels
{
    public class CreateNoteViewModel : ViewModelBase
    {
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
        
        public ICommand CreateCommand { get; }
        public ICommand UploadCommand { get; }
        
        protected CreateNoteViewModel(IViewModelFacade viewModelFacade) : base(viewModelFacade)
        {
            CreateCommand = new Command(async () => await CreateNoteAsync(), Validate);
            UploadCommand = new Command(async () => await AddAttachmentAsync());
        }

        private async Task AddAttachmentAsync()
        {
            var data = await CrossFilePicker.Current.PickFile();
            
            if (data == null || Attachments.Any(x => x.Path == data.FilePath))
            {
                return;
            }

            var attachment = new Attachment
            {
                Id = Guid.NewGuid(),
                NeedSynchronization = true,
                StatusSynchronization = SynchronizationStatusEnum.NeedInsert,
                Path = data.FilePath,
                Name = data.FileName
            };
            
            Attachments.Add(attachment);
        }

        protected override void IsValidChanged()
        {
            ((Command)CreateCommand).ChangeCanExecute();
        }

        private async Task CreateNoteAsync()
        {
            var command = MapTo<CreateNoteInSqliteCommand>(this);

            await DispatchCommandAsync(command);
            await NavigateTo("//notes");
        }
        
        private void CurrentAttachmentChangedHandler()
        {
            throw new NotImplementedException();
        }
    }
}