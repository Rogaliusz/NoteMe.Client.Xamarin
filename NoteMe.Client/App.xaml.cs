using System;
using NoteMe.Client.ViewModels;
using NoteMe.Client.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace NoteMe.Client
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();

            MainPage = new AppShell();
            Shell.Current.GoToAsync("//login");
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
