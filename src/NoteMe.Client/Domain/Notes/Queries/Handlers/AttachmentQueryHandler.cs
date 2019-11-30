using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NoteMe.Client.Domain.Notes.Queries;
using NoteMe.Client.Framework.Cqrs;
using NoteMe.Client.Sql;


namespace NoteMe.Client.Domain.Notes
{
    public class AttachmentQueryHandler : IQueryHandler<GetAttachmentQuery, Attachment>
    {
        private readonly NoteMeContextFactory _contextFactory;

        public AttachmentQueryHandler(NoteMeContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }
        
        public async Task<Attachment> HandleAsync(GetAttachmentQuery query)
        {
            using (var context = _contextFactory.CreateContext())
            {
                return await context.Attachments.FirstOrDefaultAsync(x => x.Id == query.Id);
            } 
        }
    }
}