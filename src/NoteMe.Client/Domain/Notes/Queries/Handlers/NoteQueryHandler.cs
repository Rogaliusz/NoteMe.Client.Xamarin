using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NoteMe.Client.Framework.Cqrs;
using NoteMe.Client.Framework.Mappers;
using NoteMe.Client.Sql;
using NoteMe.Common.DataTypes;
using NoteMe.Common.Domain.Notes.Dto;
using NoteMe.Common.Domain.Notes.Queries;
using NoteMe.Common.Domain.Pagination;
using N.Pag.Extensions;
using NoteMe.Client.Domain.Notes.Queries;
using NoteMe.Common.DataTypes.Domain.Notes.Queries;
using NoteMe.Common.DataTypes.Enums;

namespace NoteMe.Client.Domain.Notes
{
    public class NoteQueryHandler : 
        IQueryHandler<GetActiveNotesQuery, ICollection<Note>>,
        IQueryHandler<GetNoteQuery, Note>
    {
        private readonly INoteMeClientMapper _mapper;
        private readonly INoteMeContextFactory _factory;

        public NoteQueryHandler(
            INoteMeClientMapper mapper,
            INoteMeContextFactory factory)
        {
            _mapper = mapper;
            _factory = factory;
        }
        
        public async Task<ICollection<Note>> HandleAsync(GetActiveNotesQuery query)
        {
            using (var context = _factory.CreateContext())
            {
                var list = await context.Notes
                    .Where(x => x.Status == StatusEnum.Normal)
                    .OrderByDescending(x => x.CreatedAt)
                    .Skip(query.Page * query.PageSize)
                    .Take(query.PageSize)
                    .ToListAsync();

                return _mapper.MapTo<ICollection<Note>>(list);
            }
        }

        public Task<Note> HandleAsync(GetNoteQuery query)
        {
            using (var context = _factory.CreateContext())
            {
                return context.Notes
                    .AsNoTracking()
                    .Include(x => x.Attachments)
                    .FirstAsync(x => x.Id == query.Id);
            }
        }
    }
}