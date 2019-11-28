using System;
using System.Threading;
using System.Threading.Tasks;
using NoteMe.Client.Sql;
using NoteMe.Common.Exceptions.Client;
using NoteMe.Common.Providers;
using TinyIoC;

namespace NoteMe.Client.Domain.Synchronization.Handlers
{
    public interface ISynchronizationDispatcher
    {
        Task DispatchAsync(Synchronization synchronization, NoteMeContext context, CancellationToken ctx);
    }
    
    public class SynchronizationDispatcher : ISynchronizationDispatcher
    {
        private readonly TinyIoCContainer _container;

        public SynchronizationDispatcher(TinyIoCContainer container)
        {
            _container = container;
        }
        
        public Task DispatchAsync(Synchronization synchronization, NoteMeContext context, CancellationToken ctx)
        {
            ctx.ThrowIfCancellationRequested();
                
            var type = Type.GetType(synchronization.Type);
            var handlerType = typeof(ISynchronizationHandler<>).MakeGenericType(type);
            var handler = _container.Resolve(handlerType);

            if (handlerType == null)
            {
                throw new NoteMeNotRegisteredComponentException(handlerType);
            }

            var method = handlerType.GetMethod(nameof(ISynchronizationHandler<ISynchronizationProvider>.HandleAsync));
            return method.Invoke(handler, new object []{ synchronization, context, ctx }) as Task;
        }
    }
}