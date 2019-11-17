using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace NoteMe.Client
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();
            
            SetTabBarIsVisible(this, false);
        }

        private void AppShell_OnNavigated(object sender, ShellNavigatedEventArgs e)
        {
            Console.WriteLine("sss");
        }
    }
}
