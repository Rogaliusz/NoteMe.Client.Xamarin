using System.Threading;
using System.Threading.Tasks;
using NoteMe.Client.Sql;
using NoteMe.Common.Providers;

namespace NoteMe.Client.Domain.Synchronization.Handlers
{
    public interface ISynchronizationHandler<TEntity>
        where TEntity : IIdProvider, ISynchronizationProvider
    {
        Task HandleAsync(Synchronization synchronization, NoteMeSqlLiteContext context, CancellationToken cts);
    }
}