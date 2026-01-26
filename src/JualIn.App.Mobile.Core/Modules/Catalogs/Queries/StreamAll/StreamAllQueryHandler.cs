using JualIn.Domain.Catalogs.Entities;
using Mediator;
using SingleScope.Persistence.Abstraction;

namespace JualIn.App.Mobile.Core.Modules.Catalogs.Queries.StreamAll
{
    public class StreamAllQueryHandler(
        IReadRepository<Product> _repository
        ) : IStreamQueryHandler<StreamAllQuery, Product>
    {
        public IAsyncEnumerable<Product> Handle(StreamAllQuery query, CancellationToken cancellationToken)
            => _repository.StreamAllAsync();
    }
}
