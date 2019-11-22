
using System.Threading.Tasks;
using NoteMe.Common.Providers;

namespace NoteMe.Client.Framework.Cqrs
{
    public interface IQueryHandler
    {
        
    }
    
    public interface IQueryHandler<TQuery, TResult> : IQueryHandler
        where TQuery : IQueryProvider
    {
        Task<TResult> HandleAsync(TQuery query);
    }
}