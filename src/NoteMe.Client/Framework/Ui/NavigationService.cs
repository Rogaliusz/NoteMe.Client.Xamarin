using System;
using System.Threading.Tasks;
using NoteMe.Client.Views;
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
            if (route.Contains("login"))
            {
                Application.Current.MainPage = new LoginView();
            } 
            else if (route.Contains("register"))
            {
                Application.Current.MainPage = new RegisterView();
            } 
            else 
            {
                if (Application.Current.MainPage.GetType() != typeof(AppShell))
                {
                    Application.Current.MainPage = new AppShell();
                }

                await Shell.Current.GoToAsync(route);
            }
            


        }
    }
}