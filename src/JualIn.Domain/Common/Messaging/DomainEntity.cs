using System.Collections.Concurrent;

namespace JualIn.Domain.Common.Messaging
{
    public abstract class DomainEntity
    {
        private readonly ConcurrentQueue<IDomainEvent> _events = [];

        protected void AddDomainEvent(IDomainEvent domainEvent)
        {
            _events.Enqueue(domainEvent);
        }

        public IReadOnlyList<IDomainEvent> ConsumeEvents()
        {
            var events = _events.ToArray();

            _events.Clear();

            return events;
        }
    }
}
