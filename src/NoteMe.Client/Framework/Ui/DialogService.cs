using System.Threading.Tasks;
using NoteMe.Client.Framework.Extensions;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace NoteMe.Client.Framework.Ui
{
    public interface IDialogService
    {
        Task ShowTranslatedDialogAsync(string title, string content, string cancel = "Cancel");
        Task ShowDialogAsync(string title, string content, string cancel = "Cancel");
    }
    
    public class DialogService : IDialogService
    {
        private readonly ITranslationService _translationService;

        public DialogService(ITranslationService translationService)
        {
            _translationService = translationService;
        }
        public Task ShowTranslatedDialogAsync(string title, string content, string cancel = "Cancel")
        {
            var translatedTitle = _translationService.Translate(title);
            var translatedContent = _translationService.Translate(content);
            var translatedCancel= _translationService.Translate(cancel);

            return ShowDialogAsync(translatedTitle, translatedContent, translatedCancel);
        }

        public async Task ShowDialogAsync(string title, string content, string cancel = "Cancel")
        {
            MainThread.BeginInvokeOnMainThread(async () =>
            {
                await Application.Current.MainPage.DisplayAlert(title, content, cancel);
            });
        }
    }
}