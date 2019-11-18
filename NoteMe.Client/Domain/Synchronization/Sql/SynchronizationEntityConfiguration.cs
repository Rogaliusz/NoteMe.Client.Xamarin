using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace NoteMe.Client.Domain.Synchronization.Sql
{
    public class SynchronizationEntityConfiguration : IEntityTypeConfiguration<Synchronization>
    {
        public void Configure(EntityTypeBuilder<Synchronization> builder)
        {
            builder.HasKey(x => x.Id);
        }
    }
}