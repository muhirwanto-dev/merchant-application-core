using System.Collections.Concurrent;
using CommunityToolkit.Diagnostics;

namespace JualIn.Domain.Common.Entities
{
    public abstract class DomainEntity
    {
        private readonly ConcurrentQueue<IDomainEvent> _domainEvents = [];

        private int _lastDequeueHash;

        protected DomainEntity() { }

        protected void AddDomainEvent(IDomainEvent domainEvent)
        {
            _domainEvents.Enqueue(domainEvent);
        }

        public IReadOnlyList<IDomainEvent> ConsumeDomainEvents()
        {
            var copy = _domainEvents.ToList();

            _domainEvents.Clear();

            return copy;
        }

        public bool TryDequeue(out IDomainEvent? ev)
        {
            if (!_domainEvents.TryDequeue(out ev))
            {
                return false;
            }

            _lastDequeueHash = ev.GetHashCode();

            return true;
        }

        public void CancelDequeu(IDomainEvent ev)
        {
            Guard.Equals(ev.GetHashCode(), _lastDequeueHash);

            _domainEvents.Enqueue(ev);
        }
    }
}
