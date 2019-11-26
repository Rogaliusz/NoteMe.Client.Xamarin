using NoteMe.Client.Domain.Notes.Commands;
using NoteMe.Client.Framework.Mappers;
using NoteMe.Client.ViewModels;
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
            CreateMap<CreateNoteInSqliteCommand, Note>()
                .ReverseMap();
            CreateMap<UpdateNoteCommand, Note>()
                .ReverseMap();
            
            CreateMap<CreateNoteViewModel, CreateNoteInSqliteCommand>();

            CreateMap<Attachment, AttachmentDto>()
                .ReverseMap();
            CreateMap<CreateAttachmentCommand, Attachment>()
                .ReverseMap();
        }
    }
}