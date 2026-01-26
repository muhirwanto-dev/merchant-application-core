using JualIn.Domain.Inventories.Entities;
using Mediator;

namespace JualIn.App.Mobile.Core.Modules.Inventories.Queries.GetAll
{
    public record GetAllQuery : IQuery<IList<Inventory>>;
}
