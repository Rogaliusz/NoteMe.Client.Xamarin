using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using NoteMe.Client.Framework.Platform;
using NoteMe.Common.DataTypes;
using NoteMe.Common.DataTypes.Enums;
using Plugin.FilePicker;

namespace NoteMe.Client.Domain.Notes.Handlers
{
    public interface IAddAttachmentHandler
    {
        Task AddAsync(ICollection<Attachment> attachments);
    }
    
    public class AttachmentHandlers : IAddAttachmentHandler
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
    }
}