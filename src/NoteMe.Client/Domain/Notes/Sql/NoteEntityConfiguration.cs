using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace NoteMe.Client.Domain.Notes.Sql
{
    public class NoteEntityConfiguration : IEntityTypeConfiguration<Note>
    {
        public void Configure(EntityTypeBuilder<Note> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasOne(x => x.ActualNote)
                .WithMany(x => x.History)
                .HasForeignKey(x => x.ActualNoteId);
        }
    }
}