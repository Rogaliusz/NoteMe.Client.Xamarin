using TinyIoC;

namespace NoteMe.Client.ViewModels
{
    public class ViewModelLocator
    {
        public LoginViewModel LoginViewModel => TinyIoCContainer.Current.Resolve<LoginViewModel>();
        public RegisterViewModel RegisterViewModel => TinyIoCContainer.Current.Resolve<RegisterViewModel>();
        public NoteViewModel NoteViewModel => TinyIoCContainer.Current.Resolve<NoteViewModel>();
        public NoteCreateViewModel NoteCreateViewModel => TinyIoCContainer.Current.Resolve<NoteCreateViewModel>();
        public NoteUpdateViewModel NoteUpdateViewModel => TinyIoCContainer.Current.Resolve<NoteUpdateViewModel>();
        public ImageViewModel ImageViewModel => TinyIoCContainer.Current.Resolve<ImageViewModel>();
    }
}