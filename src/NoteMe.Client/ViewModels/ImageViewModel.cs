using System;
using System.Threading.Tasks;
using NoteMe.Client.Domain.Notes;
using NoteMe.Client.Domain.Notes.Queries;
using Xamarin.Forms;

namespace NoteMe.Client.ViewModels
{
    public class ImageViewModel : ViewModelBase
    {
        private ImageSource _image;
        private string _title;

        public ImageSource Image
        {
            get => _image;
            set => SetPropertyAndValidate(ref _image, value);
        }
        
        public string Title
        {
            get => _title;
            set => SetPropertyAndValidate(ref _title, value);
        }

        protected ImageViewModel(IViewModelFacade viewModelFacade) : base(viewModelFacade)
        {
        }

        public override async Task InitializeAsync(object parameter = null)
        {
            await base.InitializeAsync(parameter);

            var attachment =
                await DispatchQueryAsync<GetAttachmentQuery, Attachment>(
                    new GetAttachmentQuery(Guid.Parse(parameter.ToString())));

            Image = ImageSource.FromFile(attachment.Path);
            Title = attachment.Name;
        }
    }
}