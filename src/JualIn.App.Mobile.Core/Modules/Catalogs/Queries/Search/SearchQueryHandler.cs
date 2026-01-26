using JualIn.App.Mobile.Core.Infrastructure.Persistence.Abstractions;
using JualIn.Domain.Catalogs.Entities;
using Mediator;

namespace JualIn.App.Mobile.Core.Modules.Catalogs.Queries.Search
{
    public class SearchQueryHandler(
        ISearchable<Product> _repository
        ) : IQueryHandler<SearchQuery, List<Product>>
    {
        public async ValueTask<List<Product>> Handle(SearchQuery query, CancellationToken cancellationToken)
            => await _repository.SearchAsync(query.Query, cancellationToken);
    }
}
