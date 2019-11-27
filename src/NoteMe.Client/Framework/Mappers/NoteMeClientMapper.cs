using AutoMapper;

namespace NoteMe.Client.Framework.Mappers
{
    public interface INoteMeClientMapper
    {
        TDest MapTo<TDest>(object obj);

        TDest MapTo<TSrc, TDest>(TSrc obj, TDest dst);
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

        public TDest MapTo<TSrc, TDest>(TSrc obj, TDest dst)
            => _mapper.Map<TSrc, TDest>(obj, dst);
    }
}