using TinyIoC;

namespace NoteMe.Client.ViewModels
{
    public class ViewModelLocator
    {
        public LoginViewModel Login => TinyIoCContainer.Current.Resolve<LoginViewModel>();
        public RegisterViewModel Register => TinyIoCContainer.Current.Resolve<RegisterViewModel>();
        public NotesViewModel Notes => TinyIoCContainer.Current.Resolve<NotesViewModel>();
    }
}