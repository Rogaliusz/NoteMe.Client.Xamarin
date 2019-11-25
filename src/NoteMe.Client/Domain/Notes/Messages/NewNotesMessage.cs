using System.Collections.Generic;
using N.Publisher;
using NoteMe.Common.Domain.Notes.Dto;

namespace NoteMe.Client.Domain.Notes.Messages
{
    public class NewNotesMessage : NMessage
    {
        public ICollection<Note> Notes { get; set; } = new List<Note>();

        public NewNotesMessage()
        {
            
        }
        
        public NewNotesMessage(ICollection<Note> notes)
        {
            Notes = notes;
        }
        
        public NewNotesMessage(Note note)
        {
            Notes.Add(note);
        }
    }
}