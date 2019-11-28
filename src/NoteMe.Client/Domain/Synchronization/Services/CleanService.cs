using System.Threading.Tasks;
using NoteMe.Client.Sql;

namespace NoteMe.Client.Domain.Synchronization.Services
{
    public interface ICleanService
    {
        void Clean();
    }

    public class CleanService : ICleanService
    {
        private readonly ApiWebSettings _apiWebSettings;
        private readonly NoteMeContext _context;

        public CleanService(
            ApiWebSettings apiWebSettings,
            NoteMeContext context)
        {
            _apiWebSettings = apiWebSettings;
            _context = context;
        }
        public void Clean()
        {
            _context.RemoveRange(_context.Synchronizations);
            _context.RemoveRange(_context.Attachments);
            _context.RemoveRange(_context.Notes);

            _context.SaveChanges();

            _apiWebSettings.JwtDto = null;
        }
    }
}