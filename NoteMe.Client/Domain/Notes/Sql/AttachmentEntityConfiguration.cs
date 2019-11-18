using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace NoteMe.Client.Domain.Notes.Sql
{
    public class AttachmentEntityConfiguration : IEntityTypeConfiguration<Attachment>
    {
        public void Configure(EntityTypeBuilder<Attachment> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasOne(x => x.Note)
                .WithMany(x => x.Attachments)
                .HasForeignKey(x => x.NoteId);
            
        }
    }
}