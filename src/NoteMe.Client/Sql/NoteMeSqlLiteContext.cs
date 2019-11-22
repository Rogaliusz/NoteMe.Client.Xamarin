using System;
using Microsoft.EntityFrameworkCore;
using NoteMe.Client.Domain.Notes;
using NoteMe.Client.Domain.Notes.Sql;
using NoteMe.Client.Domain.Synchronization;
using NoteMe.Client.Domain.Synchronization.Sql;

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
            optionsBuilder.UseSqlite($"Filename={_settings.Path}");
            
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new NoteEntityConfiguration());
            modelBuilder.ApplyConfiguration(new AttachmentEntityConfiguration());
            modelBuilder.ApplyConfiguration(new SynchronizationEntityConfiguration());

            base.OnModelCreating(modelBuilder);
        }
    }
}