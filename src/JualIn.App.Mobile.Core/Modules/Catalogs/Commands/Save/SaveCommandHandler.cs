using ErrorOr;
using JualIn.App.Mobile.Core.Modules.Catalogs.Abstractions;
using Mediator;

namespace JualIn.App.Mobile.Core.Modules.Catalogs.Commands.Save
{
    public class SaveCommandHandler(
        IProductRepository _repository
        ) : ICommandHandler<SaveCommand, ErrorOr<Success>>
    {
        public async ValueTask<ErrorOr<Success>> Handle(SaveCommand command, CancellationToken cancellationToken)
        {
            var components = command.Product.Components;
            command.Product.Components = [];

            await _repository.UpsertAsync(command.Product, cancellationToken);
            await _repository.SaveAsync(cancellationToken); // save to update Id

            await _repository.UpdateComponentsAsync(command.Product.Id, components, cancellationToken);
            await _repository.SaveAsync(cancellationToken);

            return Result.Success;
        }
    }
}
