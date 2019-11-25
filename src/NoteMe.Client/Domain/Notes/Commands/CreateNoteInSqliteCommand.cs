using System;
using System.Collections.Generic;
using NoteMe.Common.Providers;

namespace NoteMe.Client.Domain.Notes.Commands
{
    public class CreateNoteInSqliteCommand : IIdProvider, INameProvider, ICommandProvider
    {
        public Guid Id { get; set; } 
        public string Name { get; set; }
        public string Content { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }

        public ICollection<Attachment> Attachments { get; set; }
    }
}