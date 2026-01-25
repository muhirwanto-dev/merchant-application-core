using JualIn.App.Mobile.Core.Infrastructure.Persistence.Abstractions;
using JualIn.Domain.Inventories.Entities;
using Mediator;

namespace JualIn.App.Mobile.Core.Modules.Inventories.Queries.Search
{
    public class SearchQueryHandler(
        ISearchable<Inventory> _repository
        ) : IQueryHandler<SearchQuery, List<Inventory>>
    {
        public async ValueTask<List<Inventory>> Handle(SearchQuery query, CancellationToken cancellationToken)
            => await _repository.SearchAsync(query.Query, cancellationToken);
    }
}
