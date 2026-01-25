using JualIn.Domain.Inventories.Entities;
using Mediator;
using SingleScope.Persistence.Abstraction;

namespace JualIn.App.Mobile.Core.Modules.Inventories.Queries.StreamAll
{
    public class StreamAllQueryHandler(
        IReadRepository<Inventory> _repository) : IStreamQueryHandler<StreamAllQuery, Inventory>
    {
        public IAsyncEnumerable<Inventory> Handle(StreamAllQuery query, CancellationToken cancellationToken)
            => _repository.StreamAllAsync();
    }
}
