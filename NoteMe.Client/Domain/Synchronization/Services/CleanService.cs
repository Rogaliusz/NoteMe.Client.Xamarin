using System.Threading.Tasks;
using NoteMe.Client.Sql;

namespace NoteMe.Client.Domain.Synchronization.Services
{
    public interface ICleanService
    {
        Task CleanAsync();
    }

    public class CleanService : ICleanService
    {
        private readonly NoteMeSqlLiteContext _context;

        public CleanService(NoteMeSqlLiteContext context)
        {
            _context = context;
        }
        public async Task CleanAsync()
        {
            _context.RemoveRange(_context.Synchronizations);
            _context.RemoveRange(_context.Attachments);
            _context.RemoveRange(_context.Notes);

            await _context.SaveChangesAsync();
        }
    }
}