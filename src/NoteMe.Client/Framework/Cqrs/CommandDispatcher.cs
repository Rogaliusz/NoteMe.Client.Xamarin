using System.Threading.Tasks;
using NoteMe.Common.Exceptions.Client;
using NoteMe.Common.Providers;
using TinyIoC;

namespace NoteMe.Client.Framework.Cqrs
{
    public interface ICommandDispatcher
    {
        Task DispatchAsync<TCommand>(TCommand command)
            where TCommand : ICommandProvider;
    }
    
    public class CommandDispatcher : ICommandDispatcher
    {
        private readonly TinyIoCContainer _container;
        
        public CommandDispatcher(TinyIoCContainer container)
        {
            _container = container;
        }
        
        public Task DispatchAsync<TCommand>(TCommand command) where TCommand : ICommandProvider
        {
            var type = typeof(ICommandHandler<>).MakeGenericType(typeof(TCommand));
            var handler = _container.Resolve(type) as ICommandHandler<TCommand>;

            if (handler == null)
            {
                throw new NoteMeNotRegisteredComponentException(type);
            }
            
            return handler.HandleAsync(command);
        }
    }
}