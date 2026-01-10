using JualIn.Application.Catalogs.Queries.GetProducts;
using JualIn.Application.Persistence.Repositories;
using JualIn.Domain.Catalogs.Entities;
using Wolverine.Attributes;

namespace JualIn.Application.Catalogs.Queries.SearchProducts
{
    [WolverineHandler]
    public class SearchProductsQueryHandler(
        ISearchableRepository<Product> _repository)
    {
        public Task<List<Product>> HandleAsync(SearchProductsQuery message, CancellationToken cancellation = default)
        {
            return _repository.SearchAsync(message.Query, cancellation);
        }
    }
}
