using NoteMe.Common.Providers;

namespace NoteMe.Client.Domain.Notes.Commands
{
    public class UpdateNoteSqliteCommand : ICommandProvider
    {
        public UpdateNoteSqliteCommand(Note note)
        {
            Note = note;
        }

        public Note Note { get; }
    }
}