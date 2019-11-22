using AutoMapper;

namespace NoteMe.Client.Framework.Mappers
{
    internal static class NoteMeMapperConfiguration
    {
        public static IMapper Create()
        {
            return new MapperConfiguration(opt =>
                {
                    opt.AddMaps(typeof(NoteMeClientMapper).Assembly);
                })
                .CreateMapper();
        }
    }
}