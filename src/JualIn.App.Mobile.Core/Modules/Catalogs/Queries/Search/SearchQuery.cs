using JualIn.Domain.Catalogs.Entities;
using Mediator;

namespace JualIn.App.Mobile.Core.Modules.Catalogs.Queries.Search
{
    public record SearchQuery(string Query) : IQuery<List<Product>>;
}
