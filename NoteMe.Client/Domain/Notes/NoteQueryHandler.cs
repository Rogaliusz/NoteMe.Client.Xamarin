using System.Net.Http;
using System.Threading.Tasks;
using NoteMe.Client.Framework.Cqrs;
using NoteMe.Common.DataTypes;
using NoteMe.Common.Domain.Notes.Dto;
using NoteMe.Common.Domain.Notes.Queries;
using NoteMe.Common.Domain.Pagination;

namespace NoteMe.Client.Domain.Notes
{
    public class NoteQueryHandler : IQueryHandler<GetNotesQuery, PaginationDto<NoteDto>>
    {
        private readonly ApiWebService _apiWebService;

        public NoteQueryHandler(ApiWebService apiWebService)
        {
            _apiWebService = apiWebService;
        }

        public Task<PaginationDto<NoteDto>> HandleAsync(GetNotesQuery query)
            => _apiWebService.SendAsync<PaginationDto<NoteDto>>(HttpMethod.Get, Endpoints.Notes._ + query.ToUri());
    }
}