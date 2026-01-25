using JualIn.Domain.Inventories.Entities;
using Mediator;

namespace JualIn.App.Mobile.Core.Modules.Inventories.Queries.Search
{
    public record SearchQuery(string Query) : IQuery<List<Inventory>>;
}
