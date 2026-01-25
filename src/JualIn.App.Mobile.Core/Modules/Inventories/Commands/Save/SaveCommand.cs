using ErrorOr;
using JualIn.Domain.Inventories.Entities;
using Mediator;

namespace JualIn.App.Mobile.Core.Modules.Inventories.Commands.Save
{
    public record SaveCommand(Inventory Inventory) : ICommand<ErrorOr<Success>>;
}
