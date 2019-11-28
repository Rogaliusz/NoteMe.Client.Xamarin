using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using N.Publisher;
using NoteMe.Client.Domain;
using NoteMe.Client.Domain.Synchronization.Handlers;
using NoteMe.Client.Domain.Synchronization.Services;
using NoteMe.Client.Domain.Users.Messages;
using NoteMe.Client.Framework;
using NoteMe.Client.Framework.Cqrs;
using NoteMe.Client.Framework.Mappers;
using NoteMe.Client.Framework.Messages;
using NoteMe.Client.ViewModels;
using NoteMe.Client.Views;
using TinyIoC;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Timer = System.Timers.Timer;

namespace NoteMe.Client
{
    public partial class App : Application
    {
        private NSubscription _loggedSubscription;
        private NSubscription _unloggedSubscription;
        
        public CancellationTokenSource SynchronizationTimerTokenSource { get; private set; }
        public Timer SynchronizationTimer { get; private set; }
        public TinyIoCContainer Container { get; private set; }
        public ApiWebSettings ApiWebSettings { get; private set; }
        public INavigationService NavigationService { get; private set; }
        public bool IsLogged { get; private set; }

        public App()
        {
            InitializeComponent();
            InitializeDependencies();
            InitializeStartPage();
            InitializeSubscriptions();
        }
        
        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
            if (IsLogged && (SynchronizationTimer == null || !SynchronizationTimer.Enabled))
            {   
                InitializeTimer();
            }
        }
        
        private void InitializeDependencies()
        {
            IoC.Container.RegisterDependencies();
            
            Container = TinyIoCContainer.Current;
        }
        
        private void InitializeSubscriptions()
        {
            _loggedSubscription = NPublisher.SubscribeIt<LoggedMessage>(message =>
            {
                InitializeTimer();
            });
            
            _unloggedSubscription = NPublisher.SubscribeIt<LogoutMessage>(async message =>
            {
                SynchronizationTimerTokenSource.Cancel(true);
                SynchronizationTimer.Stop();
                
                var dbCleaner = Container.Resolve<ICleanService>();
                dbCleaner.Clean();
                
                MainThread.BeginInvokeOnMainThread(InitializeStartPage);
            });
        }
        
        private void InitializeStartPage()
        {
            NavigationService = Container.Resolve<INavigationService>();
            
            ApiWebSettings = Container.Resolve<ApiWebSettings>();

            if (ApiWebSettings.JwtDto != null && ApiWebSettings.JwtDto.Expires < DateTime.UtcNow)
            {
                var dbCleaner = Container.Resolve<ICleanService>();
                dbCleaner.Clean();
            }
            
            IsLogged = ApiWebSettings.JwtDto != null;
            
            Current.MainPage = new AppShell();
                
            if (!IsLogged)
            {
                NavigationService.NavigateAsync("//login");
            }
            else
            {
                NavigationService.NavigateAsync("//notes");
                InitializeTimer();
            }
        }

        private void InitializeTimer()
        {
            SynchronizationTimerTokenSource = new CancellationTokenSource();

            SynchronizationTimer = new Timer {Interval = 1000 * 10};
            SynchronizationTimer.Elapsed += async (sender, args) => { await SynchronizeAsync().ConfigureAwait(false); };

            SynchronizationTimer.Start();

            SynchronizeAsync();
        }

        private async Task SynchronizeAsync()
        {
            var syncService = Container.Resolve<ISynchronizationService>();
            await syncService.SynchronizeAsync(SynchronizationTimerTokenSource.Token);
        }
    }
}
