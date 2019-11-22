using NoteMe.Common.Providers;

namespace NoteMe.Client.Domain.Notes.Queries
{
    public class GetActiveNotesQuery : IQueryProvider
    {
        public int Page { get; set; }
        public int PageSize { get; set; } = 50;
    }
}