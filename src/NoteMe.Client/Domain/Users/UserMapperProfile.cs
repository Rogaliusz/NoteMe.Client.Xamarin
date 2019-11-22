using NoteMe.Client.Framework.Mappers;
using NoteMe.Client.ViewModels;
using NoteMe.Common.Domain.Users.Commands;

namespace NoteMe.Client.Domain.Users
{
    public class UserMapperProfile : NoteMeClientMapperProfile
    {
        public UserMapperProfile()
        {
            CreateMap<LoginViewModel, LoginCommand>();
            CreateMap<RegisterViewModel, UserRegisterCommand>();
            CreateMap<UserRegisterCommand, LoginCommand>();
        }
    }
}