using System;
using System.Linq;
using System.Threading;
using AutoMapper;
using NoteMe.Client.Domain;
using NoteMe.Client.Domain.Synchronization.Services;
using NoteMe.Client.Framework;
using NoteMe.Client.Framework.Cqrs;
using NoteMe.Client.Framework.Mappers;
using NoteMe.Client.ViewModels;
using NoteMe.Client.Views;
using TinyIoC;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Timer = System.Timers.Timer;

namespace NoteMe.Client
{
    public partial class App : Application
    {
        public CancellationTokenSource TokenSource { get; private set; }
        public Timer SynchronizationTimer { get; private set; }
        public TinyIoCContainer Container { get; private set; }
        public bool IsLogged { get; private set; }

        public App()
        {
            TokenSource = new CancellationTokenSource();
            
            InitializeComponent();
            InitializeDependencies();
            InitializeTimer();
            InitializeStartPage();
        }

        private void InitializeDependencies()
        {
            Container = TinyIoCContainer.Current;
            
            Container.AutoRegister();

            var mapper = NoteMeMapperConfiguration.Create();
            Container.Register<IMapper>(mapper);
            
            RegisterAll(typeof(ICommandHandler));
            RegisterAll(typeof(IQueryHandler));
        }

        private void RegisterAll(Type type)
        {
            var types = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(x => x.GetTypes())
                .Where( x => type.IsAssignableFrom(x));

            foreach (var impType in types.Where(x => !x.IsAbstract))
            {
                var interfaces = impType.GetInterfaces();
                foreach (var @interface in interfaces)
                {
                    Container.Register(@interface, impType);
                }
            }
        }
        
        private void InitializeTimer()
        {
            SynchronizationTimer = new Timer {Interval = 1000 * 60};
            SynchronizationTimer.Elapsed += async (sender, args) =>
            {
                var syncService = Container.Resolve<ISynchronizationService>();
                await syncService.SynchronizeAsync(TokenSource.Token);
            };
        }
        
        private void InitializeStartPage()
        {
            var navigationService = Container.Resolve<INavigationService>();
            var settings = Container.Resolve<ApiWebSettings>();

            IsLogged = settings.JwtDto == null;

            if (!IsLogged)
            {
                MainPage = new LoginView();
            }
            else
            {
                MainPage = new AppShell();
                navigationService.NavigateAsync("//main");
                SynchronizationTimer.Start();
            }
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
