using System;
using System.Collections.Generic;
using N.Publisher;
using NoteMe.Client.Domain.Users.Messages;
using NoteMe.Client.Views;
using Xamarin.Forms;

namespace NoteMe.Client
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            Routing.RegisterRoute("login", typeof(LoginView));
            Routing.RegisterRoute("register", typeof(RegisterView));
            Routing.RegisterRoute("notes/details", typeof(UpdateNoteView));
            Routing.RegisterRoute("notes/attachment", typeof(ImageView));

            InitializeComponent();

            SetTabBarIsVisible(this, false);
        }

        private void AppShell_OnNavigated(object sender, ShellNavigatedEventArgs e)
        {
            Console.WriteLine("sss");
        }

        private void LogoutMenuItem_OnClicked(object sender, EventArgs e)
        {
            NPublisher.PublishIt<LogoutMessage>();
        }
    }
}
