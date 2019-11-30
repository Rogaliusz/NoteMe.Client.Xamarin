using System;
using System.Linq;
using AutoMapper;
using FluentValidation;
using NoteMe.Client;
using NoteMe.Client.Domain.Notes.Handlers;
using NoteMe.Client.Domain.Synchronization.Handlers;
using NoteMe.Client.Domain.Synchronization.Services;
using NoteMe.Client.Framework;
using NoteMe.Client.Framework.Cqrs;
using NoteMe.Client.Framework.Device;
using NoteMe.Client.Framework.Extensions;
using NoteMe.Client.Framework.Mappers;
using NoteMe.Client.Framework.Ui;
using NoteMe.Client.Framework.Validation;
using NoteMe.Client.Sql;
using NoteMe.Client.ViewModels;
using NoteMe.Client.Views;
using TinyIoC;

namespace IoC
{
    public class Container
    {
        private static TinyIoCContainer _container;
        
        public static void RegisterDependencies()
        {
            _container = TinyIoCContainer.Current;

            RegisterSynchronizations();
            RegisterCqrs();
            RegisterValidations();
            RegisterUi();
            RegisterViewModels();
            RegisterSql();
            RegisterMapper();
            RegisterFramework();
            RegisterOthers();

            RegisterAll(typeof(ISynchronizationHandler));
            RegisterAll(typeof(ICommandHandler));
            RegisterAll(typeof(IQueryHandler));
            RegisterAll(typeof(IValidator));
        }

        private static void RegisterSynchronizations()
        {
            _container.Register<ISynchronizationService, SynchronizationService>();
            _container.Register<ISynchronizationDispatcher, SynchronizationDispatcher>();
        }

        private static void RegisterCqrs()
        {
            _container.Register<ICommandDispatcher, CommandDispatcher>();
            _container.Register<IQueryDispatcher, QueryDispatcher>();
        }

        private static void RegisterValidations()
        {
            _container.Register<IValidationDispatcher, ValidationDispatcher>();
        }

        private static void RegisterUi()
        {
            _container.Register<IDialogService, DialogService>();
            _container.Register<INavigationService, NavigationService>();
        }

        private static void RegisterViewModels()
        {
            _container.Register<LoginViewModel>();
            _container.Register<RegisterViewModel>();
            _container.Register<NoteViewModel>();
            _container.Register<NoteCreateViewModel>();
            _container.Register<NoteUpdateViewModel>();
            _container.Register<ImageViewModel>();
            
            _container.Register<IViewModelFacade, ViewModelFacade>();
        }

        private static void RegisterSql()
        {
            var settings = _container.Resolve<SqliteSettings>();
            var factory = new NoteMeContextFactory(settings);
            _container.Register<INoteMeContextFactory>(factory);
        }

        private static void RegisterMapper()
        {
            var mapper = NoteMeMapperConfiguration.Create();
            _container.Register<IMapper>(mapper);
            _container.Register<INoteMeClientMapper, NoteMeClientMapper>();
        }

        private static void RegisterFramework()
        {
            _container.Register<IPermissionService, PermissionService>();
            _container.Register<IGeolocationService, GeolocationService>();
            _container.Register<ICameraService, CameraService>();
        }

        private static void RegisterOthers()
        {
            _container.Register<ICleanService, CleanService>();
            _container.Register<IAttachmentHandler, AttachmentHandlers>();
            _container.Register<ITranslationService, TranslationService>();
        }

        private static void RegisterAll(Type type)
        {
            var types = new [] {typeof(App).Assembly, typeof(IValidator).Assembly}
                .SelectMany(x => x.GetTypes())
                .Where( x => type.IsAssignableFrom(x));

            foreach (var impType in types.Where(x => !x.IsAbstract))
            {
                var interfaces = impType.GetInterfaces();
                foreach (var @interface in interfaces)
                {
                    _container.Register(@interface, impType);
                }
            }
        }
    }
}