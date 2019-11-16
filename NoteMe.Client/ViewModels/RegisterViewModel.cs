using System;
using System.Threading.Tasks;
using System.Windows.Input;
using NoteMe.Client.Views;
using Xamarin.Forms;

namespace NoteMe.Client.ViewModels
{
    public class RegisterViewModel : BaseViewModel
    {
        private string _email;
        private string _password;
        private string _confirmPassword;
     
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
        
        public string ConfirmPassword
        {
            get => _confirmPassword;
            set => SetProperty(ref _confirmPassword, value);
        }
        
        public ICommand RegisterCommand { get; set; }
        public ICommand GoToLoginCommand { get; set; }

        public RegisterViewModel()
        {
            RegisterCommand = new Command(async () => await RegisterAsync());
            GoToLoginCommand = new Command(execute: async () => await GoToLoginAsync());
        }

        private async Task RegisterAsync()
        {
            Console.WriteLine("sss");
        }

        private async Task GoToLoginAsync()
        {
            Application.Current.MainPage = new LoginView();
        }
    }
}