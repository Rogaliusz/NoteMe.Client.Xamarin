using System.Net.Http;
using System.Threading.Tasks;
using NoteMe.Client.Framework.Cqrs;
using NoteMe.Client.Framework.Mappers;
using NoteMe.Client.Framework.Messages;
using NoteMe.Common.DataTypes;
using NoteMe.Common.Domain.Users.Commands;
using NoteMe.Common.Domain.Users.Dto;
using Xamarin.Forms;

namespace NoteMe.Client.Domain.Users
{
    public class UserCommandHandler : 
        ICommandHandler<UserRegisterCommand>,
        ICommandHandler<LoginCommand>
        
    {
        private readonly INoteMeClientMapper _mapper;
        private readonly ApiWebSettings _apiWebSettings;
        private readonly ApiWebService _apiWebService;

        public UserCommandHandler(
            INoteMeClientMapper mapper,
            ApiWebSettings apiWebSettings,
            ApiWebService apiWebService)
        {
            _mapper = mapper;
            _apiWebSettings = apiWebSettings;
            _apiWebService = apiWebService;
        }
        
        public async Task HandleAsync(UserRegisterCommand command)
        {
            await _apiWebService.SendAsync<UserDto>(HttpMethod.Post, Endpoints.Users, command);
            
            var loginCommand = _mapper.MapTo<LoginCommand>(command);
            await HandleAsync(loginCommand);
        }
        
        public async Task HandleAsync(LoginCommand command)
        {
            var result = await _apiWebService.SendAsync<JwtDto>(HttpMethod.Post, Endpoints.Login, command);
            _apiWebSettings.JwtDto = result;
            
            MessagingCenter.Send(this, Messages.Logged);
        }
    }
}