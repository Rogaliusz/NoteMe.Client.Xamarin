using System;
using System.Threading.Tasks;
using System.Windows.Input;
using NoteMe.Client.Views;
using Xamarin.Forms;

namespace NoteMe.Client.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        private string _email;
        private string _password; 
     
        public string Email
        {
            get => _email;
            set => SetProperty(ref _email, value);
        }

        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }
        
        public ICommand LoginCommand { get; set; }
        public ICommand GoToRegisterCommand { get; set; }

        public LoginViewModel()
        {
            LoginCommand = new Command(async () => await LoginAsync());
            GoToRegisterCommand = new Command(async () => await GoToRegisterAsync());
        }
        
        private async Task LoginAsync()
        {
            Console.WriteLine("sss");
        }
        
        private async Task GoToRegisterAsync()
        {
            Application.Current.MainPage = new RegisterView();
        }
    }
}
