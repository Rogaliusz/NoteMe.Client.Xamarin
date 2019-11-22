using System.Threading.Tasks;
using NoteMe.Common.Providers;

namespace NoteMe.Client.Framework.Cqrs
{
    public interface ICommandHandler
    {
        
    }
    public interface ICommandHandler<TCommand> : ICommandHandler
        where TCommand : ICommandProvider
    {
        Task HandleAsync(TCommand command);
    }
    
}