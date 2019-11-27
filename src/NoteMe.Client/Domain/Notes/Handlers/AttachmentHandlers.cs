using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using NoteMe.Client.Framework.Platform;
using NoteMe.Common.DataTypes;
using NoteMe.Common.DataTypes.Enums;
using Plugin.FilePicker;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace NoteMe.Client.Domain.Notes.Handlers
{
    public interface IAttachmentHandler
    {
        Task AddAsync(ICollection<Attachment> attachments);

        Task OpenAsync(Attachment note);
    }

    public class AttachmentHandlers : IAttachmentHandler
    {
        private readonly IFilePathService _filePathService;

        public AttachmentHandlers(IFilePathService filePathService)
        {
            _filePathService = filePathService;
        }
        
        public async Task AddAsync(ICollection<Attachment> attachments)
        {
            var data = await CrossFilePicker.Current.PickFile();
            var newPath = Path.Combine(_filePathService.GetFilesDirectory(), data?.FileName ?? string.Empty);
            
            if (data == null || attachments.Any(x => x.Path == newPath))
            {
                return;
            }

            var attachment = new Attachment
            {
                Id = Guid.NewGuid(),
                NeedSynchronization = true,
                StatusSynchronization = SynchronizationStatusEnum.NeedInsert,
                Path = newPath,
                Name = data.FileName
            };
            
            File.WriteAllBytes(newPath, data.DataArray);

            attachments.Add(attachment);
        }

        public async Task OpenAsync(Attachment attachment)
        {
            var openFileRequest = new OpenFileRequest
            {
                File = new ReadOnlyFile(attachment.Path)
            };

            await Launcher.OpenAsync(openFileRequest);
        }
    }
}