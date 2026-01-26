using JualIn.App.Mobile.Core.Infrastructure.Persistence.Abstractions;
using JualIn.Domain.Catalogs.Entities;
using Mediator;

namespace JualIn.App.Mobile.Core.Modules.Catalogs.Queries.GetCategories
{
    public class GetCategoriesQueryHandler(
        ICategoryResolver<Product> _repository
        ) : IQueryHandler<GetCategoriesQuery, string[]>
    {
        public async ValueTask<string[]> Handle(GetCategoriesQuery query, CancellationToken cancellationToken)
            => await _repository.GetCategoriesAsync(cancellationToken);
    }
}
