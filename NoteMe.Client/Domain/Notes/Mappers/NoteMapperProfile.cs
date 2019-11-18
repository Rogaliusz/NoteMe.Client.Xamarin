using NoteMe.Client.Framework.Mappers;
using NoteMe.Common.Domain.Notes.Commands;
using NoteMe.Common.Domain.Notes.Dto;

namespace NoteMe.Client.Domain.Notes.Mappers
{
    public class NoteMapperProfile : NoteMeClientMapperProfile
    {
        public NoteMapperProfile()
        {
            CreateMap<Note, NoteDto>()
                .ReverseMap();
            CreateMap<CreateNoteCommand, Note>()
                .ReverseMap();
            CreateMap<UpdateNoteCommand, Note>()
                .ReverseMap();
            
            CreateMap<Attachment, AttachmentDto>();
            CreateMap<CreateAttachmentCommand, Attachment>();
        }
    }
}