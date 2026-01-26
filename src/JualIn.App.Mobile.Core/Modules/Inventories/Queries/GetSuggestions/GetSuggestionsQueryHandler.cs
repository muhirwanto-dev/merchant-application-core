using JualIn.App.Mobile.Core.Infrastructure.Persistence.Abstractions;
using JualIn.Domain.Inventories.Entities;
using Mediator;

namespace JualIn.App.Mobile.Core.Modules.Inventories.Queries.GetSuggestions
{
    public class GetSuggestionsQueryHandler(
        ISearchable<Inventory> _repository
        ) : IQueryHandler<GetSuggestionsQuery, string[]>
    {
        public async ValueTask<string[]> Handle(GetSuggestionsQuery query, CancellationToken cancellationToken)
            => await _repository.GetSuggestionsAsync(cancellationToken);
    }
}
