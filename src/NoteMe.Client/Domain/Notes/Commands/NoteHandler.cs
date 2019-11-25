using System;
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
    public class NoteHandler : ICommandHandler<CreateNoteCommand>
    {
        private readonly NoteMeClientMapper _mapper;
        private readonly NoteMeSqlLiteContext _context;

        public NoteHandler(
            NoteMeClientMapper mapper,
            NoteMeSqlLiteContext context)
        {
            _mapper = mapper;
            _context = context;
        }
        
        public async Task HandleAsync(CreateNoteCommand command)
        {
            var note = _mapper.MapTo<Note>(command);
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

            await _context.SaveChangesAsync();
            
            NPublisher.PublishIt(new NewNotesMessage(note));
        }
    }
}