using System;
using NoteMe.Common.DataTypes.Enums;
using NoteMe.Common.Providers;

namespace NoteMe.Client.Domain.Notes
{
    public class Attachment : 
        IIdProvider,
        ICreatedAtProvider,
        ISynchronizationProvider
    {
        public string Name { get; set; }
        public Guid Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? LastSynchronization { get; set; }
        public bool NeedSynchronization { get; set; }
        public SynchronizationStatusEnum StatusSynchronization { get; set; }
        public string Path { get; set; }

        public Guid NoteId { get; set; }
        public Note Note { get; set; }
    }
}