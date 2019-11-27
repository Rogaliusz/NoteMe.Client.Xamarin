using System.Collections.ObjectModel;
using System.Windows.Input;
using NoteMe.Client.Domain.Notes;
using NoteMe.Client.Framework.Validation;

namespace NoteMe.Client.ViewModels.Forms
{
    public interface INoteForm : IValidationForm
    {
        string Name { get; set; }
        string Tags { get; set; }
        string Content { get; set; }
        
        Attachment CurrentAttachment { get; set; }
        ObservableCollection<Attachment> Attachments { get; set; }
        
        ICommand SaveCommand { get; }
        ICommand UploadCommand { get; }
    }
}