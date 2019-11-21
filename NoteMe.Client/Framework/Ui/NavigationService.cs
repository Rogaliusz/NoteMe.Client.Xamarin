using System;
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
            if (MainThread.IsMainThread)
            {
                await InternalNavigateAsync(route);
            }
            else
            {
                MainThread.BeginInvokeOnMainThread(async () => await InternalNavigateAsync(route));
            }
        }

        private static async Task InternalNavigateAsync(string route)
        {
            Shell.Current.FlyoutIsPresented = !(route.Contains("login") || route.Contains("register"));
            
            if (!Shell.Current.FlyoutIsPresented)
            {
                Shell.Current.FlyoutBehavior = FlyoutBehavior.Disabled;
                Shell.Current.IsTabStop = false;
            }
            else
            {
                Shell.Current.FlyoutBehavior = FlyoutBehavior.Flyout;
                Shell.Current.IsTabStop = true;
            }

            await Shell.Current.GoToAsync(route);
        }
    }
}