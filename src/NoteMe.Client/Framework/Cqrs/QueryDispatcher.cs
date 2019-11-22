
using System.Threading.Tasks;
using NoteMe.Common.Exceptions.Client;
using NoteMe.Common.Providers;
using TinyIoC;

namespace NoteMe.Client.Framework.Cqrs
{
    public interface IQueryDispatcher
    {
        Task<TResult> DispatchAsync<TQuery, TResult>(TQuery query)
            where TQuery : IQueryProvider;
    }
    
    public class QueryDispatcher : IQueryDispatcher
    {
        private readonly TinyIoCContainer _container;

        public QueryDispatcher(TinyIoCContainer container)
        {
            _container = container;
        }
        
        public Task<TResult> DispatchAsync<TQuery, TResult>(TQuery query) where TQuery : IQueryProvider
        {
            var type = typeof(IQueryHandler<,>).MakeGenericType(typeof(TQuery), typeof(TResult));
            var handler = _container.Resolve(type) as IQueryHandler<TQuery, TResult>;
            if (handler == null)
            {
                throw new NoteMeNotRegisteredComponentException(type);
            }
            
            return handler.HandleAsync(query);
        }
    }
}