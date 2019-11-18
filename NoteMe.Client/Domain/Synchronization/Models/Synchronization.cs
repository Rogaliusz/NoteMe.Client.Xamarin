using System;
using NoteMe.Common.Providers;

namespace NoteMe.Client.Domain.Synchronization
{
    public class Synchronization : IIdProvider, 
        ICreatedAtProvider
    {
        public Guid Id { get; set; }
        public string Type { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime LastSynchronization { get; set; }
        public int Order { get; set; }
    }
}