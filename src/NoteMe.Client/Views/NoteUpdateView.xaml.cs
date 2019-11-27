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
    [QueryProperty("Id", "id")]
    public partial class UpdateNoteView : ContentPage
    {
        public string Id
        {
            set
            {
                var viewModel = BindingContext as NoteUpdateViewModel;
                viewModel?.InitializeAsync(value);
            }
        }

        public UpdateNoteView()
        {
            InitializeComponent();
        }
    }
}