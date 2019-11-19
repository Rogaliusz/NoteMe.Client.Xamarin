using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace NoteMe.Client.Framework
{
    public interface INavigationService
    {
        Task NavigateAsync(string route);
    }
    
    public class NavigationService : INavigationService
    {
        public NavigationService()
        {
            
        }

        public async Task NavigateAsync(string route)
        {
            MainThread.BeginInvokeOnMainThread(async () =>
            {
                if (Application.Current.MainPage.GetType() != typeof(AppShell))
                {
                    Application.Current.MainPage = new AppShell();
                }

                await Shell.Current.GoToAsync(route);
            });
        }
    }
}