using System;
using System.IO;
using NoteMe.Client.Framework.Platform;

namespace NoteMe.Client.iOS.Platform
{
    public class FilePathService : IFilePathService
    {
        public string GetFilesDirectory()
        {
            var path = Path.Combine(Environment.GetFolderPath((Environment.SpecialFolder.MyDocuments)),
                "..",
                "Library",
                "files");
            
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            return path;
        }
    }
}