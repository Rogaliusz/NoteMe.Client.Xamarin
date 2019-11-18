using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Xsl;
using Microsoft.EntityFrameworkCore;
using NoteMe.Client.Domain.Synchronization.Handlers;
using NoteMe.Client.Framework.Mappers;
using NoteMe.Client.Sql;
using NoteMe.Common.DataTypes;
using NoteMe.Common.Domain.Notes.Commands;
using NoteMe.Common.Domain.Notes.Dto;
using NoteMe.Common.Domain.Notes.Queries;
using NoteMe.Common.Domain.Pagination;
using NPag.Queries;

namespace NoteMe.Client.Domain.Notes.Synchronization
{
    public class NoteSynchronizationHandler : ISynchronizationHandler<Note>
    {
        private readonly INoteMeClientMapper _mapper;
        private readonly ApiWebService _webService;

        public NoteSynchronizationHandler(
            INoteMeClientMapper mapper,
            ApiWebService webService)
        {
            _mapper = mapper;
            _webService = webService;
        }
        
        public async Task HandleAsync(Domain.Synchronization.Synchronization synchronization, NoteMeSqlLiteContext context, CancellationToken cts)
        {
            cts.ThrowIfCancellationRequested();

            var syncDate = synchronization.LastSynchronization;
            var hasMore = false;

            do
            {
                var filterBy = $"{nameof(Note.CreatedAt)} > {syncDate}";
                var orderBy = $"{nameof(Note.CreatedAt)}";
                var query = new GetNotesQuery()
                    .SetNormalWhere(filterBy)
                    .SetNormalOrderBy(orderBy);

                var notes = await _webService.SendAsync<PaginationDto<NoteDto>>(HttpMethod.Get,
                    Endpoints.Notes._ + query.ToUri());

                hasMore = notes.Data.Count == query.PageSize;
                syncDate = notes.Data.Max(x => x.CreatedAt);

                foreach (var noteDto in notes.Data)
                {
                    var note = await context.Notes.FirstOrDefaultAsync(x => x.Id == noteDto.Id, cts);

                    if (note != null) continue;
                    
                    note = _mapper.MapTo<Note>(noteDto);
                    await context.AddRangeAsync(note);

                    foreach (var historical in noteDto.OldNotes)
                    {
                        var history = await context.Notes.AsTracking().FirstOrDefaultAsync(x => x.Id == historical.Id, cts);
                        history.ActualNoteId = note.Id;
                    }

                }
            } while (hasMore);

            await context.SaveChangesAsync(cts);
            
            cts.ThrowIfCancellationRequested();
            
            var toInserts = await context.Notes.AsTracking().Where(x => x.CreatedAt == default(DateTime)).ToListAsync(cts);
            var insertsCommands = _mapper.MapTo<ICollection<CreateNoteCommand>>(toInserts);
            foreach (var command in insertsCommands)
            {
                var created = await _webService.SendAsync<NoteDto>(HttpMethod.Get, Endpoints.Notes._, command);
                var entity = toInserts.First(x => x.Id == command.Id);
                entity.LastSynchronization = DateTime.UtcNow;
                entity.CreatedAt = created.CreatedAt;
                entity.Status = created.Status;
            }

            await context.SaveChangesAsync(cts);
        }
    }
}