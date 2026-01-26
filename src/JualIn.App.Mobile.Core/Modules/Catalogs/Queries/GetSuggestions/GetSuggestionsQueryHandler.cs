using JualIn.App.Mobile.Core.Infrastructure.Persistence.Abstractions;
using JualIn.Domain.Catalogs.Entities;
using Mediator;

namespace JualIn.App.Mobile.Core.Modules.Catalogs.Queries.GetSuggestions
{
    public class GetSuggestionsQueryHandler(
        ISearchable<Product> _repository
        ) : IQueryHandler<GetSuggestionsQuery, string[]>
    {
        public async ValueTask<string[]> Handle(GetSuggestionsQuery query, CancellationToken cancellationToken)
            => await _repository.GetSuggestionsAsync(cancellationToken);
    }
}
