using ErrorOr;
using Mediator;

namespace JualIn.App.Mobile.Core.Modules.Inventories.Commands.Delete
{
    public record DeleteCommand(long EntityId) : ICommand<ErrorOr<Success>>;
}
