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
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public ICollection<Attachment> Attachments { get; set; }
    }
}