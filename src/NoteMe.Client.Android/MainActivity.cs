using System;
using System.IO;
using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using NoteMe.Client.Sql;
using TinyIoC;
using Environment = System.Environment;
using NoteMe.Client.Droid.Platform;
using NoteMe.Client.Framework.Platform;
using Plugin.CurrentActivity;

namespace NoteMe.Client.Droid
{
    [Activity(Label = "NoteMe.Client", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);
            
            global::Xamarin.Forms.Forms.SetFlags("CollectionView_Experimental");
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            CrossCurrentActivity.Current.Init(this, savedInstanceState);

            this.InitializeSqlite();
            this.InitializePlaformServices();

            LoadApplication(new App());


        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        private void InitializeSqlite()
        {
            var settings = new SqliteSettings
            {
                Path = Path.Combine(Environment.GetFolderPath((Environment.SpecialFolder.MyDocuments)),
                    "database.sqlite")
            };

            TinyIoCContainer.Current.Register(settings);
        }

        private void InitializePlaformServices()
        {
            TinyIoCContainer.Current.Register<IFilePathService, FilePathService>();
        }
    }
}