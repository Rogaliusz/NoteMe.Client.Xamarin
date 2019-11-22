using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using N.Publisher;
using NoteMe.Client.Domain.Users.Messages;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace NoteMe.Client.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LogoutView : ContentPage
    {
        public LogoutView()
        {
            NPublisher.PublishIt<LogoutMessage>();
            
            InitializeComponent();
        }
    }
}