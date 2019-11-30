using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using N.Publisher;
using NoteMe.Client.Domain.Notes.Messages;
using NoteMe.Client.Framework.Cqrs;
using NoteMe.Client.Framework.Device;
using NoteMe.Client.Framework.Mappers;
using NoteMe.Client.Sql;
using NoteMe.Common.DataTypes.Enums;
using NoteMe.Common.Domain.Notes.Commands;
using Xamarin.Essentials;

namespace NoteMe.Client.Domain.Notes.Commands
{
    public class NoteHandler : ICommandHandler<CreateNoteInSqliteCommand>,
        ICommandHandler<UpdateNoteSqliteCommand>
    {
        private readonly IGeolocationService _geolocationService;
        private readonly INoteMeClientMapper _mapper;
        private readonly INoteMeContextFactory _factory;

        public NoteHandler(
            IGeolocationService geolocationService,
            INoteMeClientMapper mapper,
            INoteMeContextFactory factory)
        {
            _geolocationService = geolocationService;
            _mapper = mapper;
            _factory = factory;
        }
        
        public async Task HandleAsync(CreateNoteInSqliteCommand command)
        {
            using (var context = _factory.CreateContext())
            {
                var note = _mapper.MapTo<Note>(command);
                note.Id = Guid.NewGuid();
                note.NeedSynchronization = true;
                note.StatusSynchronization = SynchronizationStatusEnum.NeedInsert;

                try
                {
                    var (lot, lat) = await _geolocationService.GeLocationAsync();
                    note.Latitude = lat;
                    note.Longitude = lot;
                }
                catch (PermissionException)
                {

                }

                note.CreatedAt = DateTime.UtcNow;

                await context.AddAsync(note);
                await context.SaveChangesAsync();

                NPublisher.PublishIt(new NewNotesMessage(note));
            }
        }

        public async Task HandleAsync(UpdateNoteSqliteCommand command)
        {
            using (var context = _factory.CreateContext())
            {
                var note = command.Note;
                note.StatusSynchronization = SynchronizationStatusEnum.NeedUpdate;
                
                context.Update(note);

                foreach (var attachment in note.Attachments)
                {
                    if (context.Attachments.Any(x => x.Id == attachment.Id))
                    {
                        context.Update(attachment);
                    }
                    else
                    {
                        context.Add(attachment);
                    }
                }

                await context.SaveChangesAsync();
            }
        }
    }
}