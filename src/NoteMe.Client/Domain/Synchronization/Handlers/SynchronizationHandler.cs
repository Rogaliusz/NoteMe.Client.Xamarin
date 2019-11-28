using System.Threading;
using System.Threading.Tasks;
using NoteMe.Client.Sql;
using NoteMe.Common.Providers;

namespace NoteMe.Client.Domain.Synchronization.Handlers
{
    public interface ISynchronizationHandler
    {

    }

    public interface ISynchronizationHandler<TEntity> : ISynchronizationHandler
        where TEntity : IIdProvider, ISynchronizationProvider
    {
        Task HandleAsync(Synchronization synchronization, NoteMeContext context, CancellationToken cts);
    }
}