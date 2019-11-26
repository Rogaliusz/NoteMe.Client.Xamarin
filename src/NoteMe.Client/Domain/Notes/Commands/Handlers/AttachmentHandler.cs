using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using NoteMe.Client.Framework.Cqrs;
using NoteMe.Common.DataTypes;
using NoteMe.Common.Domain.Notes.Commands;
using NoteMe.Common.Domain.Notes.Dto;

namespace NoteMe.Client.Domain.Notes.Commands
{
    public class AttachmentHandler : 
        ICommandHandler<CreateAttachmentCommand>
    {
        private readonly ApiWebService _apiWebService;

        public AttachmentHandler(ApiWebService apiWebService)
        {
            _apiWebService = apiWebService;
        }
        
        public async Task HandleAsync(CreateAttachmentCommand command)
        {
            await _apiWebService.SendAsync<AttachmentDto>(HttpMethod.Post, Endpoints.Attachments._, command);
        }
        
    }
}