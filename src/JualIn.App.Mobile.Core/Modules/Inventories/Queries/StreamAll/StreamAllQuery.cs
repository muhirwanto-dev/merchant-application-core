using JualIn.Domain.Inventories.Entities;
using Mediator;

namespace JualIn.App.Mobile.Core.Modules.Inventories.Queries.StreamAll
{
    public record StreamAllQuery : IStreamQuery<Inventory>;
}
