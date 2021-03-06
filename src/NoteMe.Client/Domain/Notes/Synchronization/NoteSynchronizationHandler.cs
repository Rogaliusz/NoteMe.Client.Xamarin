using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Xsl;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using NoteMe.Client.Domain.Synchronization.Handlers;
using NoteMe.Client.Framework.Mappers;
using NoteMe.Client.Sql;
using NoteMe.Common.DataTypes;
using NoteMe.Common.Domain.Notes.Commands;
using NoteMe.Common.Domain.Notes.Dto;
using NoteMe.Common.Domain.Notes.Queries;
using NoteMe.Common.Domain.Pagination;
using N.Publisher;
using NoteMe.Client.Domain.Notes.Messages;
using NoteMe.Common.DataTypes.Enums;
using NoteMe.Common.Providers;

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
        
        public async Task HandleAsync(Domain.Synchronization.Synchronization synchronization, NoteMeContext context, CancellationToken cts)
        {
            await UpdateAllNotesAsync(context, cts);
            await FetchAllResultsAsync(synchronization, context, cts);
            await SendAllNotesAsync(context, cts);
        }
        
        private async Task FetchAllResultsAsync(Domain.Synchronization.Synchronization synchronization, NoteMeContext context, CancellationToken cts)
        {
            cts.ThrowIfCancellationRequested();

            var syncDate = synchronization.LastSynchronization;
            var hasMore = false;
            var allNotes = new List<Note>();

            do
            {
                var filterBy = $@"{nameof(Note.CreatedAt)} > ""{syncDate}""";
                var orderBy = $"{nameof(Note.CreatedAt)}-desc";
                var query = new GetNotesQuery()
                    .SetNormalWhere(filterBy)
                    .SetNormalOrderBy(orderBy);

                var notes = await _webService.SendAsync<PaginationDto<NoteDto>>(HttpMethod.Get,
                    Endpoints.Notes._ + query.ToUri());

                if (!notes.Data.Any() && !allNotes.Any())
                {
                    return;
                }
                
                hasMore = notes.Data.Count == query.PageSize;

                foreach (var noteDto in notes.Data)
                {
                    var note = await context.Notes.FirstOrDefaultAsync(x => x.Id == noteDto.Id, cts);
                    if (note != null)
                    {
                        note.Content = noteDto.Content;
                        note.Tags = noteDto.Tags;
                        note.Name = noteDto.Name;
                    }
                    else
                    {
                        note = _mapper.MapTo<Note>(noteDto);

                        if (note.Status == StatusEnum.Normal)
                        {
                            allNotes.Add(note);
                        }

                        await context.AddRangeAsync(note);
                    }
                }
            } while (hasMore);
            
            await context.SaveChangesAsync(cts);
            
            NPublisher.PublishIt(new NewNotesMessage(allNotes));
        }
        
        private async Task SendAllNotesAsync(NoteMeContext context, CancellationToken cts)
        {
            await SendNotesToApi<CreateNoteCommand>(
                (cmd) => _webService.SendAsync<NoteDto>(HttpMethod.Post, Endpoints.Notes._, cmd),
                note => note.StatusSynchronization == SynchronizationStatusEnum.NeedInsert,
                context,
                cts);
        }
        
        private async Task UpdateAllNotesAsync(NoteMeContext context, CancellationToken cts)
        {
            await SendNotesToApi<UpdateNoteCommand>(
                (cmd) => _webService.SendAsync<NoteDto>(HttpMethod.Put, Endpoints.Notes._ + cmd.Id, cmd),
                note => note.StatusSynchronization == SynchronizationStatusEnum.NeedUpdate,
                context,
                cts);
        }

        private async Task SendNotesToApi<TCommand>(Func<TCommand, Task<NoteDto>> sendAsync,
            Expression<Func<Note, bool>> predicate,
            NoteMeContext context, 
            CancellationToken cts)
            where TCommand : IIdProvider
        {
            cts.ThrowIfCancellationRequested();

            var toInserts = await context.Notes
                .AsTracking()
                .Where(predicate)
                .ToListAsync(cts);
            
            var insertsCommands = _mapper.MapTo<ICollection<TCommand>>(toInserts);
            foreach (var command in insertsCommands)
            {
                var created = await sendAsync.Invoke(command);
                var entity = toInserts.First(x => x.Id == command.Id);
                
                entity.LastSynchronization = DateTime.UtcNow;
                entity.CreatedAt = created.CreatedAt;
                entity.Status = created.Status;
                entity.StatusSynchronization = SynchronizationStatusEnum.Ok;
            }

            await context.SaveChangesAsync(cts);
        }
    }
}