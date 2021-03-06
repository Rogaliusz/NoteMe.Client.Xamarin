using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NoteMe.Client.Domain.Notes;
using NoteMe.Client.Domain.Synchronization.Handlers;
using NoteMe.Client.Sql;
using TinyIoC;
using Xamarin.Essentials;

namespace NoteMe.Client.Domain.Synchronization.Services
{
    public interface ISynchronizationService
    {
        Task SynchronizeAsync(CancellationToken cts);
    }
    
    public class SynchronizationService : ISynchronizationService
    {
        private static readonly SemaphoreSlim _synchronizationSemaphore = new SemaphoreSlim(1);
        private readonly INoteMeContextFactory _contextFactory;
        private readonly ISynchronizationDispatcher _dispatcher;
        private readonly TinyIoCContainer _container;

        public SynchronizationService(
            INoteMeContextFactory contextFactory,
            ISynchronizationDispatcher dispatcher,
            TinyIoCContainer container)
        {
            _contextFactory = contextFactory;
            _dispatcher = dispatcher;
            _container = container;
        }
        
        public async Task SynchronizeAsync(CancellationToken cts)
        {
            await _synchronizationSemaphore.WaitAsync(cts);

            try
            {
                var current = Connectivity.NetworkAccess;

                if (current != NetworkAccess.Internet)
                {
                    return;
                }

                using (var dbContext = _contextFactory.CreateContext())
                {
                    var existed = await dbContext.Synchronizations.ToListAsync(cts);
                    var toInsert = GetDefaultSynchronizations().Where(x => !existed.Any(e => e.Type == x.Type));
                    await dbContext.Synchronizations.AddRangeAsync(toInsert, cts);
                    var lastSyncs = await dbContext.Synchronizations.AsTracking().ToListAsync(cts);

                    foreach (var x in lastSyncs)
                    {
                        await SynchronizeAsync(x, dbContext, cts);
                    }

                    await dbContext.SaveChangesAsync(cts);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            finally
            {
                _synchronizationSemaphore.Release();
            }

        }

        private ICollection<Synchronization> GetDefaultSynchronizations()
        {
            return new List<Synchronization>
            {
                new Synchronization
                {
                    Id = Guid.NewGuid(),
                    Type = typeof(Note).FullName,
                    Order = 0
                },
                new Synchronization
                {
                    Id = Guid.NewGuid(),
                    Type = typeof(Attachment).FullName,
                    Order = 1
                }
            };
        }

        private async Task SynchronizeAsync(
            Synchronization synchronization, 
            NoteMeContext context,
            CancellationToken cts)
        {
            await _dispatcher.DispatchAsync(synchronization, context, cts);
        }
    }
}