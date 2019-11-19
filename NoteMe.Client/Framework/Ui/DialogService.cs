using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace NoteMe.Client.Framework.Ui
{
    public interface IDialogService
    {
        Task ShowDialogAsync(string title, string content, string cancel = "Cancel");
    }
    
    public class DialogService : IDialogService
    {
        public async Task ShowDialogAsync(string title, string content, string cancel = "Cancel")
        {
            MainThread.BeginInvokeOnMainThread(async () =>
            {
                await Application.Current.MainPage.DisplayAlert(title, content, cancel);
            });
        }
    }
}