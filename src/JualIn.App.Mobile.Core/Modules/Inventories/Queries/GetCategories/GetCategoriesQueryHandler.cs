using JualIn.App.Mobile.Core.Infrastructure.Persistence.Abstractions;
using JualIn.Domain.Inventories.Entities;
using Mediator;

namespace JualIn.App.Mobile.Core.Modules.Inventories.Queries.GetCategories
{
    public class GetCategoriesQueryHandler(
        ICategoryResolver<Inventory> _repository
        ) : IQueryHandler<GetCategoriesQuery, string[]>
    {
        public async ValueTask<string[]> Handle(GetCategoriesQuery query, CancellationToken cancellationToken)
            => await _repository.GetCategoriesAsync(cancellationToken);
    }
}
