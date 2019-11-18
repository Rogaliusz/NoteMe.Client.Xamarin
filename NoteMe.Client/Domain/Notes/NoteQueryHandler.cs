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
using NPag.Extensions;

namespace NoteMe.Client.Domain.Notes
{
    public class NoteQueryHandler : IQueryHandler<GetNotesQuery, PaginationDto<NoteDto>>
    {
        private readonly INoteMeClientMapper _mapper;
        private readonly NoteMeSqlLiteContext _noteMeSqlLiteContext;

        public NoteQueryHandler(
            INoteMeClientMapper mapper,
            NoteMeSqlLiteContext noteMeSqlLiteContext)
        {
            _mapper = mapper;
            _noteMeSqlLiteContext = noteMeSqlLiteContext;
        }

        public async Task<PaginationDto<NoteDto>> HandleAsync(GetNotesQuery query)
        {
            var queryable = _noteMeSqlLiteContext.Notes.FilterBy(query);
            var count = await queryable.CountAsync();
            var data = await queryable.TransformBy(query).ToListAsync();

            var paginationDto = new PaginationDto<Note>
            {
                TotalCount = count,
                Data = data
            };

            return _mapper.MapTo<PaginationDto<NoteDto>>(paginationDto);
        }
    }
}