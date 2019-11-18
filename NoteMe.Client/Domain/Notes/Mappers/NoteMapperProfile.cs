using NoteMe.Client.Framework.Mappers;
using NoteMe.Common.Domain.Notes.Commands;
using NoteMe.Common.Domain.Notes.Dto;

namespace NoteMe.Client.Domain.Notes.Mappers
{
    public class NoteMapperProfile : NoteMeClientMapperProfile
    {
        public NoteMapperProfile()
        {
            CreateMap<Note, NoteDto>();
            CreateMap<CreateNoteCommand, Note>();
            CreateMap<UpdateNoteCommand, Note>();
            
            CreateMap<Attachment, AttachmentDto>();
            CreateMap<CreateAttachmentCommand, Attachment>();
        }
    }
}