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

            BackgroundImageSource = ImageSource.FromFile("logged_background.png");
            

        }
    }
}
