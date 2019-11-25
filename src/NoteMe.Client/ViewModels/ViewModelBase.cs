using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using FluentValidation;
using NoteMe.Common.Providers;
using IQueryProvider = NoteMe.Common.Providers.IQueryProvider;

namespace NoteMe.Client.ViewModels
{
    public class ViewModelBase : INotifyPropertyChanged
    {
        private readonly IViewModelFacade _viewModelFacade;

        private string _lastValidationErrorMessage = "";
        private IValidator _validator;
        
        private bool _isBusy = false;
        private bool _isValid = false;
        private string _title = string.Empty;
        private string _error = string.Empty;
        
        
        public bool IsBusy
        {
            get { return _isBusy; }
            set { SetProperty(ref _isBusy, value); }
        }
        
        public bool IsValid
        {
            get { return _isValid; }
            set { SetProperty(ref _isValid, value, onChanged:IsValidChanged); }
        }
        
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        public string Error
        {
            get { return _error; }
            set { SetProperty(ref _error, value); }
        }

        protected ViewModelBase(IViewModelFacade viewModelFacade)
        {
            _viewModelFacade = viewModelFacade;
        }
        
        protected async Task DispatchCommandAsync<TCommmand>(TCommmand commmand)
            where TCommmand : ICommandProvider
        {
            IsBusy = true;

            try
            {
                await _viewModelFacade.CommandDispatcher.DispatchAsync(commmand)
                    .ConfigureAwait(false);
            }
            finally
            {
                IsBusy = false;
            }
        }
        
        protected async Task<TResult> DispatchQueryAsync<TQuery, TResult>(TQuery commmand)
            where TQuery : IQueryProvider
        {
            IsBusy = true;

            try
            {
                return await _viewModelFacade.QueryDispatcher.DispatchAsync<TQuery, TResult>(commmand)
                    .ConfigureAwait(false);
            }
            finally
            {
                IsBusy = false;
            }
        }
        
        protected virtual void IsValidChanged()
        {
            
        }

        public async virtual Task InitializeAsync(object parameter = null)
        {
            
        }
        
        protected virtual bool Validate()
        {
            if (_validator == null)
            {
                _validator = _viewModelFacade.ValidationDispatcher.GetValidator(this.GetType());
            }

            var validationResult = _validator.ValidateAsync(this);
            var error = validationResult.Result.Errors.FirstOrDefault();
            
            if (error == null)
            {
                Error = string.Empty;
                return IsValid = true;
            }
            
            var translatedObjects = error.FormattedMessagePlaceholderValues
                .Where(x => x.Key == "PropertyName")
                .Select(x => (object) Translate(x.Value.ToString()));

            var errorMessage = error.ErrorMessage + translatedObjects.FirstOrDefault();

            if (_lastValidationErrorMessage == errorMessage)
            { 
                return IsValid;
            }

            _lastValidationErrorMessage = errorMessage;
            
            Error = string.Format(Translate(error.ErrorMessage), translatedObjects.ToArray());
            IsValid = string.IsNullOrEmpty(Error);

            return IsValid;
        }

        protected Task ShowDialogAsync(string title, string content, string cancel = "Cancel")
            => _viewModelFacade.DialogService.ShowDialogAsync(title, content, cancel);

        protected TDest MapTo<TDest>(object obj) 
            => _viewModelFacade.Mapper.MapTo<TDest>(obj);

        protected Task NavigateTo(string route)
            => _viewModelFacade.NavigationService.NavigateAsync(route);

        protected string Translate(string text)
            => _viewModelFacade.TranslationService.Translate(text);
        

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            var changed = PropertyChanged;
            if (changed == null)
                return;

            changed.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        protected bool SetPropertyAndValidate<T>(
            ref T backingStore,
            T value,
            [CallerMemberName] string propertyName = "")
            => SetProperty(ref backingStore, value, propertyName, () => Validate());
            
        
        protected bool SetProperty<T>(ref T backingStore, T value,
            [CallerMemberName]string propertyName = "",
            Action onChanged = null)
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
                return false;

            backingStore = value;
            onChanged?.Invoke();
            OnPropertyChanged(propertyName);
            return true;
        }
    }
}
