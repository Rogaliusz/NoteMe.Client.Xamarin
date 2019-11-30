using System;
using System.Threading.Tasks;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;

namespace NoteMe.Client.Framework.Device
{
    public interface IPermissionService
    {
        Task CheckPermissionAsync(Permission permission);
    }

    public class PermissionService : IPermissionService
    {
        public PermissionService()
        {
        }

        public async Task CheckPermissionAsync(Permission permission)
        {
            var status = await CrossPermissions.Current.CheckPermissionStatusAsync(permission);

            if (status != PermissionStatus.Granted)
            {
                await CrossPermissions.Current.RequestPermissionsAsync(permission);
            }
        }
    }
}
