using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using NoteMe.Common.Providers;

namespace NoteMe.Client.ViewModels
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        private readonly IViewModelFacade _viewModelFacade;
        
        bool _isBusy = false;
        string _title = string.Empty;
        
        public bool IsBusy
        {
            get { return _isBusy; }
            set { SetProperty(ref _isBusy, value); }
        }
        
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        protected BaseViewModel(IViewModelFacade viewModelFacade)
        {
            _viewModelFacade = viewModelFacade;
        }

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

        public async virtual Task InitializeAsync(object parameter = null)
        {
            
        }

        protected TDest MapTo<TDest>(object obj) 
            => _viewModelFacade.Mapper.MapTo<TDest>(obj);

        protected Task NavigateTo(string route)
            => _viewModelFacade.NavigationService.NavigateAsync(route);

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
    }
}
