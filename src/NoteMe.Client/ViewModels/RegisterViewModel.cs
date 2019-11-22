using System;
using System.Threading.Tasks;
using System.Windows.Input;
using NoteMe.Client.Views;
using NoteMe.Common.Domain.Users.Commands;
using Xamarin.Forms;

namespace NoteMe.Client.ViewModels
{
    public class RegisterViewModel : ViewModelBase
    {
        private string _email;
        private string _password;
        private string _confirmPassword;
     
        public string Email
        {
            get => _email;
            set => SetPropertyAndValidate(ref _email, value);
        }

        public string Password
        {
            get => _password;
            set => SetPropertyAndValidate(ref _password, value);
        }
        
        public string ConfirmPassword
        {
            get => _confirmPassword;
            set => SetPropertyAndValidate(ref _confirmPassword, value);
        }
        
        public ICommand RegisterCommand { get; set; }
        public ICommand GoToLoginCommand { get; set; }

        public RegisterViewModel(IViewModelFacade viewModelFacade) : base(viewModelFacade)
        {
            RegisterCommand = new Command(async () => await RegisterAsync(), Validate);
            GoToLoginCommand = new Command(execute: async () => await GoToLoginAsync());
        }

        private async Task RegisterAsync()
        {
            var command = MapTo<UserRegisterCommand>(this);
            await DispatchCommandAsync(command).ConfigureAwait(false);
        }

        private Task GoToLoginAsync()
            => NavigateTo("//login");

        protected override void IsValidChanged()
        {
            base.IsValidChanged();
            
            ((Command)RegisterCommand).ChangeCanExecute();
        }
    }
}