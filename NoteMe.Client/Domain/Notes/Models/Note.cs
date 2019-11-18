using System;
using System.Collections.Generic;
using NoteMe.Common.DataTypes.Enums;
using NoteMe.Common.Providers;

namespace NoteMe.Client.Domain.Notes
{
    public class Note : IIdProvider,
        ICreatedAtProvider,
        IModifiedAtProvider,
        ISynchronizationProvider
    {
        public Guid Id { get; set; }
        public StatusEnum Status { get; set; }
        public string Name { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ModifiedAt { get; set; }
        public DateTime? LastSynchronization { get; set; }
        public bool NeedSynchronization { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
     
        public Guid? ActualNoteId { get; set; }
        
        public Note ActualNote { get; set; }
        
        public ICollection<Note> History { get; set; }
        public ICollection<Attachment> Attachments { get; set; }
    }
}