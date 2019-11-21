using System.Collections.Generic;
using N.Publisher;
using NoteMe.Common.Domain.Notes.Dto;

namespace NoteMe.Client.Domain.Notes.Messages
{
    public class NewNotesMessage : NMessage
    {
        public ICollection<NoteDto> Notes { get; set; }

        public NewNotesMessage()
        {
            
        }
        
        public NewNotesMessage(ICollection<NoteDto> notes)
        {
            Notes = notes;
        }


    }
}