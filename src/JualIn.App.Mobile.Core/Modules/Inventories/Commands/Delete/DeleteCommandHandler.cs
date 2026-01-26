using ErrorOr;
using JualIn.Domain.Inventories.Entities;
using Mediator;
using SingleScope.Persistence.Abstraction;

namespace JualIn.App.Mobile.Core.Modules.Inventories.Commands.Delete
{
    public class DeleteCommandHandler(
        IReadWriteRepository<Inventory> _repository
        ) : ICommandHandler<DeleteCommand, ErrorOr<Success>>
    {
        public async ValueTask<ErrorOr<Success>> Handle(DeleteCommand command, CancellationToken cancellationToken)
        {
            await _repository.DeleteAsync(command.EntityId, cancellationToken);
            await _repository.SaveAsync(cancellationToken);

            return Result.Success;
        }
    }
}
