using NoteMe.Common.Domain.Users.Dto;
using NoteMe.Common.Extensions;
using NoteMe.Common.Services.Json;
using Xamarin.Essentials;

namespace NoteMe.Client.Domain
{
    public class ApiWebSettings
    {
        private const string AddressKey = "api_address";
        private const string TokenKey = "api_token";

        private JwtDto _tokenInstance;

        public string Address
        {
            get => Preferences.Get(AddressKey, "https://localhost:5001");
            set => Preferences.Set(AddressKey, value);
        }

        public JwtDto JwtDto
        {
            get
            {
                if (_tokenInstance != null)
                {
                    return _tokenInstance;
                }

                var serializedToken = Preferences.Get(TokenKey, string.Empty);
                if (serializedToken.IsEmpty())
                {
                    return null;
                }

                return _tokenInstance = JsonSerializeService.Deserialize<JwtDto>(serializedToken);
            }
            set
            {
                _tokenInstance = value;

                var serialized = JsonSerializeService.Serialize(value);
                Preferences.Set(TokenKey, serialized);
            }
        }
    }
}