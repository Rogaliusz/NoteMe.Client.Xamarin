using System;
using NoteMe.Common.Providers;

namespace NoteMe.Client.Domain.Notes.Queries
{
    public class GetAttachmentQuery : IQueryProvider
    {
        public GetAttachmentQuery(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; }
    }
}