using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace NoteMe.Client.Views.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AttachmentControl : StackLayout
    {
        public AttachmentControl()
        {
            InitializeComponent();
        }
    }
}