using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NoteMe.Client.Domain.Synchronization.Handlers;
using NoteMe.Client.Framework.Cqrs;
using NoteMe.Client.Framework.Mappers;
using NoteMe.Client.Sql;
using NoteMe.Common.DataTypes;
using NoteMe.Common.DataTypes.Enums;
using NoteMe.Common.Domain.Notes.Commands;
using NoteMe.Common.Domain.Notes.Dto;
using NoteMe.Common.Domain.Notes.Queries;
using NoteMe.Common.Domain.Pagination;

namespace NoteMe.Client.Domain.Notes.Synchronization
{
    public class AttachmentSynchronizationHandler : ISynchronizationHandler<Attachment>
    {
        private readonly ApiWebService _apiWebService;
        private readonly INoteMeClientMapper _mapper;

        public AttachmentSynchronizationHandler(
            ApiWebService apiWebService,
            INoteMeClientMapper mapper)
        {
            _apiWebService = apiWebService;
            _mapper = mapper;
        }
        
        public async Task HandleAsync(
            Domain.Synchronization.Synchronization synchronization, 
            NoteMeSqlLiteContext context, 
            CancellationToken cts)
        {
            await FetchAllAttachmentsAsync(synchronization, context, cts);
            await CreateAllAttachmentsAsync(synchronization, context, cts);
            await UploadAllAttachmentsAsync(context, cts);
            //await DownloadAllAttachmentsAsync(context, cts);
        }
        
        private async Task FetchAllAttachmentsAsync(Domain.Synchronization.Synchronization synchronization, NoteMeSqlLiteContext context,
            CancellationToken cts)
        {
            cts.ThrowIfCancellationRequested();

            var syncDate = synchronization.LastSynchronization;
            var hasMore = false;

            do
            {
                var filterBy = $@"{nameof(Attachment.CreatedAt)} > ""{syncDate}""";
                var orderBy = $"{nameof(Attachment.CreatedAt)}-desc";
                var query = new GetAttachmentQuery()
                    .SetNormalWhere(filterBy)
                    .SetNormalOrderBy(orderBy);

                var items = await _apiWebService.SendAsync<PaginationDto<AttachmentDto>>(HttpMethod.Get,
                    Endpoints.Attachments._ + query.ToUri());

                if (!items.Data.Any())
                {
                    return;
                }

                hasMore = items.Data.Count == query.PageSize;

                foreach (var itemDto in items.Data)
                {
                    var item = await context.Attachments.FirstOrDefaultAsync(x => x.Id == itemDto.Id, cts);
                    if (item != null) continue;

                    item = _mapper.MapTo<Attachment>(itemDto);
                    item.StatusSynchronization = SynchronizationStatusEnum.Ok;

                    await context.AddRangeAsync(item);
                }
            } while (hasMore);

            await context.SaveChangesAsync(cts);
        }
        
        private async Task CreateAllAttachmentsAsync(Domain.Synchronization.Synchronization synchronization, NoteMeSqlLiteContext context, CancellationToken cts)
        {
            cts.ThrowIfCancellationRequested();

            var toInserts = await context.Attachments
                .AsTracking()
                .Where(x => x.StatusSynchronization == SynchronizationStatusEnum.NeedInsert)
                .ToListAsync(cts);
            
            var insertsCommands = _mapper.MapTo<ICollection<CreateAttachmentCommand>>(toInserts);
            foreach (var command in insertsCommands)
            {
                var created = await _apiWebService.SendAsync<AttachmentDto>(HttpMethod.Post, Endpoints.Attachments._, command);
                var entity = toInserts.First(x => x.Id == command.Id);
                
                entity.LastSynchronization = DateTime.UtcNow;
                entity.CreatedAt = created.CreatedAt;
                entity.StatusSynchronization = SynchronizationStatusEnum.NeedUpload;
            }

            await context.SaveChangesAsync(cts);
        }
        
        private async Task UploadAllAttachmentsAsync(
            NoteMeSqlLiteContext context, 
            CancellationToken cts)
        {
            cts.ThrowIfCancellationRequested();

            var attachmentsToUpload = await context.Attachments
                .Where(x => x.StatusSynchronization == SynchronizationStatusEnum.NeedUpload)
                .AsTracking()
                .ToListAsync(cts);

            foreach (var attachment in attachmentsToUpload)
            {
                await _apiWebService.UploadAsync(Endpoints.Attachments.Upload, attachment.Path, attachment.Id);
                attachment.StatusSynchronization = SynchronizationStatusEnum.Ok;
                await context.SaveChangesAsync(cts);
                cts.ThrowIfCancellationRequested();
            }
        }


        private async Task DownloadAllAttachmentsAsync(NoteMeSqlLiteContext context, CancellationToken cts)
        {
            cts.ThrowIfCancellationRequested();

            var attachmentsWithoutPath = await context.Attachments
                .Where(x => string.IsNullOrEmpty(x.Path))
                .AsTracking()
                .ToListAsync(cts);

            foreach (var attachment in attachmentsWithoutPath)
            {
                cts.ThrowIfCancellationRequested();

                var fileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "temp.txt");
                
                await context.SaveChangesAsync(cts);
            }
        }
    }
}