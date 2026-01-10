using JualIn.Domain.Common;
using JualIn.Domain.Common.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Wolverine;

namespace JualIn.Infrastructure.Persistence
{
    internal class DomainEventsDbInterceptor(
        IMessageBus _bus
        ) : SaveChangesInterceptor
    {
        public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
        {
            PublishDomainEvents(eventData.Context);

            return base.SavingChanges(eventData, result);
        }

        public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
        {
            await PublishDomainEventsAsync(eventData.Context, cancellationToken);

            return await base.SavingChangesAsync(eventData, result, cancellationToken);
        }

        private void PublishDomainEvents(DbContext? context)
        {
            var entities = context?.ChangeTracker.Entries<Entity>().ToArray();
            if (entities != null)
            {
                foreach (var entity in entities)
                {
                    while (entity.Entity.TryDequeue(out IDomainEvent? ev))
                    {
                        _ = _bus.PublishAsync(ev);
                    }
                }
            }
        }

        private async Task PublishDomainEventsAsync(DbContext? context, CancellationToken cancellation = default)
        {
            var entities = context?.ChangeTracker.Entries<Entity>().ToArray();
            if (entities != null)
            {
                foreach (var entity in entities)
                {
                    while (entity.Entity.TryDequeue(out IDomainEvent? ev))
                    {
                        if (cancellation.IsCancellationRequested)
                        {
                            break;
                        }

                        await _bus.PublishAsync(ev);
                    }
                }
            }
        }
    }
}
