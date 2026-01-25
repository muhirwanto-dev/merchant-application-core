using ErrorOr;
using JualIn.App.Mobile.Core.Modules.Inventories.Abstractions;
using Mediator;

namespace JualIn.App.Mobile.Core.Modules.Inventories.Commands.Save
{
    public class SaveCommandHandler(
        IInventoryRepository _repository
        ) : ICommandHandler<SaveCommand, ErrorOr<Success>>
    {
        public async ValueTask<ErrorOr<Success>> Handle(SaveCommand command, CancellationToken cancellationToken)
        {
            await _repository.UpsertAsync(command.Inventory, cancellationToken);
            await _repository.SaveAsync(cancellationToken);

            return Result.Success;
        }
    }
}
