using System;
using System.Threading.Tasks;
using System.Windows.Input;
using NoteMe.Client.Views;
using NoteMe.Common.Domain.Users.Commands;
using Xamarin.Forms;

namespace NoteMe.Client.ViewModels
{
    public class LoginViewModel : ViewModelBase
    {
        private string _email;
        private string _password;
        private bool _isLoginEnabled;
     
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

        public ICommand LoginCommand { get; set; }
        public ICommand GoToRegisterCommand { get; set; }

        public LoginViewModel(
            
            IViewModelFacade viewModelFacade) : base(viewModelFacade)
        {
            LoginCommand = new Command(async () => await LoginAsync(), Validate);
            GoToRegisterCommand = new Command(async () => await GoToRegisterAsync());
        }
        
        private async Task LoginAsync()
        {
            var command = MapTo<LoginCommand>(this);

            try
            {
                await DispatchCommandAsync(command)
                    .ConfigureAwait(false);

                await NavigateTo("//main");
            }
            catch (UnauthorizedAccessException)
            {
                await ShowDialogAsync("Incorrect credentials", "Please try again with correct credentials");
            }
        }

        protected override void IsValidChanged()
        {
            base.IsValidChanged();

            ((Command)LoginCommand).ChangeCanExecute();
        }

        private Task GoToRegisterAsync()
            => NavigateTo("//register");
    }
}
