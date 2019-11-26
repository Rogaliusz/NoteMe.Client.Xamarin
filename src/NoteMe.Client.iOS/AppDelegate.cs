using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Foundation;
using NoteMe.Client.Framework.Platform;
using NoteMe.Client.iOS.Platform;
using NoteMe.Client.Sql;
using TinyIoC;
using UIKit;

namespace NoteMe.Client.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        //
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            SQLitePCL.Batteries_V2.Init();

            global::Xamarin.Forms.Forms.SetFlags("CollectionView_Experimental");
            global::Xamarin.Forms.Forms.Init();

            var settings = CreateSqlSettings();

            LoadApplication(new App());

            TinyIoCContainer.Current.Register(settings);
            TinyIoCContainer.Current.Register<IFilePathService, FilePathService>();

            return base.FinishedLaunching(app, options);
        }

        private static SqliteSettings CreateSqlSettings()
        {
            var libPath = Path.Combine(Environment.GetFolderPath((Environment.SpecialFolder.MyDocuments)),
                "..",
                "Library",
                "data");

            if (!Directory.Exists(libPath))
            {
                Directory.CreateDirectory(libPath);
            }

            var settings = new SqliteSettings
            {
                Path = Path.Combine(libPath,
                    "database.sqlite")
            };
            return settings;
        }
    }
}
