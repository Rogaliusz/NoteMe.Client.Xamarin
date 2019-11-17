using System;
using System.Linq;
using AutoMapper;
using NoteMe.Client.Domain;
using NoteMe.Client.Framework;
using NoteMe.Client.Framework.Cqrs;
using NoteMe.Client.Framework.Mappers;
using NoteMe.Client.ViewModels;
using NoteMe.Client.Views;
using TinyIoC;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace NoteMe.Client
{
    public partial class App : Application
    {
        public TinyIoCContainer Container { get; private set; }

        public App()
        {
            InitializeComponent();
            InitializeDependencies();
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
        
        private void InitializeStartPage()
        {
            var navigationService = Container.Resolve<INavigationService>();
            var settings = Container.Resolve<ApiWebSettings>();

            if (settings.JwtDto == null)
            {
                MainPage = new LoginView();
            }
            else
            {
                MainPage = new AppShell();
                navigationService.NavigateAsync("//main");
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
