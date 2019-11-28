using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using NoteMe.Client.Framework.Extensions;
using NoteMe.Client.Framework.Platform;
using NoteMe.Client.Framework.Ui;
using NoteMe.Common.DataTypes;
using NoteMe.Common.DataTypes.Enums;
using NoteMe.Common.Extensions;
using Plugin.FilePicker;
using Plugin.Media;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace NoteMe.Client.Domain.Notes.Handlers
{
    public interface IAttachmentHandler
    {
        Task<Attachment> TakePhotoAsync(ICollection<Attachment> attachments);

        Task OpenAsync(Attachment note);
    }

    public class AttachmentHandlers : IAttachmentHandler
    {
        private readonly IDialogService _dialogService;
        private readonly IFilePathService _filePathService;

        public AttachmentHandlers(
            IDialogService dialogService,
            IFilePathService filePathService)
        {
            _dialogService = dialogService;
            _filePathService = filePathService;
        }
        
        public async Task<Attachment> TakePhotoAsync(ICollection<Attachment> attachments)
        {
            await CrossMedia.Current.Initialize();
            
            var cameraStatus = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Camera);
            var storageStatus = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Storage);

            if (cameraStatus != PermissionStatus.Granted || storageStatus != PermissionStatus.Granted)
            {
                await CrossPermissions.Current.RequestPermissionsAsync(Permission.Camera);
                await CrossPermissions.Current.RequestPermissionsAsync(Permission.Storage);
            }
            
            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
                await _dialogService.ShowDialogAsync("NoCameraTitle", "NoCameraContent", "OK");
                return null;
            }
            
            var photo = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions() { });
            if (photo == null)
            {
                await _dialogService.ShowDialogAsync("NoCameraTitle", "NoCameraContent", "OK");
                return null;
            }

            using (var streamImage = photo.GetStream())
            {
                var name = Path.GetFileName(photo.Path);
                var newPath = Path.Combine(_filePathService.GetFilesDirectory(), name ?? string.Empty);
                
                var attachment = new Attachment
                {
                    Id = Guid.NewGuid(),
                    NeedSynchronization = true,
                    StatusSynchronization = SynchronizationStatusEnum.NeedInsert,
                    Path = newPath,
                    Name = name
                };

                
                File.WriteAllBytes(newPath, streamImage.ReadFully());

                attachments.Add(attachment);

                return attachment;
            }
        }

        public async Task OpenAsync(Attachment attachment)
        {
            if (!File.Exists(attachment.Path))
            {
                await _dialogService.ShowTranslatedDialogAsync("WaitForDownloadTitle", "WaitForDownloadContent");
                return;
            }
            
            var uri = new Uri(attachment.Path);
            await Launcher.OpenAsync(uri);
        }
    }
}