using System;
using NoteMe.Common.Providers;

namespace NoteMe.Client.Domain.Notes.Commands
{
    public class UploadAttachmentCommand : ICommandProvider, IIdProvider
    {
        public Guid Id { get; set; }
        public string FullPath { get; set; }
    }
}