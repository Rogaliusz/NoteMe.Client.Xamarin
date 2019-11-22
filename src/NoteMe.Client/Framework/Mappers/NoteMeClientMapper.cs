using AutoMapper;

namespace NoteMe.Client.Framework.Mappers
{
    public interface INoteMeClientMapper
    {
        TDest MapTo<TDest>(object obj);
    }
    
    public class NoteMeClientMapper : INoteMeClientMapper
    {
        private readonly IMapper _mapper;

        public NoteMeClientMapper(IMapper mapper)
        {
            _mapper = mapper;
        }

        public TDest MapTo<TDest>(object obj)
            => _mapper.Map<TDest>(obj);
    }
}