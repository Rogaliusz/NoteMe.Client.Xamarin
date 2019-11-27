using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace NoteMe.Client.Views.Forms
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NoteForm : RelativeLayout
    {
        public NoteForm()
        {
            InitializeComponent();
        }
    }
}