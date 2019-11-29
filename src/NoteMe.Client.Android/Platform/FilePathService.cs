using System;
using System.IO;
using NoteMe.Client.Framework.Platform;
using Xamarin.Forms;

namespace NoteMe.Client.Droid.Platform
{
    public class FilePathService : IFilePathService
    {
        public FilePathService() 
        {
        }

        public string GetFilesDirectory()
        {
            var path = Path.Combine(Environment.GetFolderPath((Environment.SpecialFolder.ApplicationData)), "files");

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            return path;
        }
    }
}
