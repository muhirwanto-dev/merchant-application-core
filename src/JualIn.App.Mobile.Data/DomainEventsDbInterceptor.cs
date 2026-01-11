using JualIn.Domain.Common;
using JualIn.Domain.Common.Entities;
using Mediator;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace JualIn.App.Mobile.Data
{
    internal class DomainEventsDbInterceptor(
        IMediator _mediator
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
                        _ = _mediator.Publish(ev!);
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

                        await _mediator.Publish(ev!);
                    }
                }
            }
        }
    }
}
