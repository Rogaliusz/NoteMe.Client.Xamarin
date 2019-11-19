using Microsoft.EntityFrameworkCore;
using NoteMe.Client.Domain.Notes;
using NoteMe.Client.Domain.Synchronization;

namespace NoteMe.Client.Sql
{
    public class NoteMeSqlLiteContext : DbContext
    {
        private readonly SqliteSettings _settings;
        
        public DbSet<Attachment> Attachments { get; set; }
        public DbSet<Note> Notes { get; set; }
        public DbSet<Synchronization> Synchronizations { get; set; }
        
        public NoteMeSqlLiteContext(SqliteSettings settings)
        {
            _settings = settings;

            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(_settings.Path);
            
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(NoteMeSqlLiteContext).Assembly);
            
            base.OnModelCreating(modelBuilder);
        }
    }
}