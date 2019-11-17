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
            MainThread.BeginInvokeOnMainThread(async () => { await Shell.Current.GoToAsync(route); });
        }
    }
}