using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using N.Publisher;
using NoteMe.Client.Domain.Notes.Messages;
using NoteMe.Client.Framework.Cqrs;
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
        private readonly INoteMeClientMapper _mapper;
        private readonly INoteMeContextFactory _factory;

        public NoteHandler(
            INoteMeClientMapper mapper,
            INoteMeContextFactory factory)
        {
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
                    var geo = await Geolocation.GetLastKnownLocationAsync();
                    note.Latitude = (decimal) geo.Latitude;
                    note.Longitude = (decimal) geo.Longitude;
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
                command.Note.StatusSynchronization = SynchronizationStatusEnum.NeedUpdate;
                context.Update(command.Note);
                await context.SaveChangesAsync();
            }
        }
    }
}