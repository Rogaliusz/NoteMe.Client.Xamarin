using System;
using System.Threading.Tasks;
using Plugin.Permissions.Abstractions;
using Xamarin.Essentials;

namespace NoteMe.Client.Framework.Device
{
    public interface IGeolocationService
    {
        Task<(double, double)> GeLocationAsync();
    }

    public class GeolocationService : IGeolocationService
    {
        private readonly IPermissionService _permissionService;

        public GeolocationService(IPermissionService permissionService)
        {
            _permissionService = permissionService;
        }

        /// <summary>
        /// Get Location async
        /// </summary>
        /// <returns>lot then lat</returns>

        public async Task<(double, double)> GeLocationAsync()
        {
            await _permissionService.CheckPermissionAsync(Permission.Location);

            var geo = await Geolocation.GetLastKnownLocationAsync();

            return (geo.Longitude, geo.Latitude);
        }
    }
}
