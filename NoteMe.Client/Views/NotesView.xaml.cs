using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NoteMe.Client.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace NoteMe.Client.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NotesView : ContentPage
    {
        public NotesView()
        {
            BindingContextChanged += HandleBindingContextChanged;
            InitializeComponent();
        }

        private void HandleBindingContextChanged(object sender, EventArgs e)
        {
            var viewModel = BindingContext as NoteViewModel;
            viewModel?.InitializeAsync();
        }

        private void ItemsView_OnScrollToRequested(object sender, ScrollToRequestEventArgs e)
        {
        }
    }
}