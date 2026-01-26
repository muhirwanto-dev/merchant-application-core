using ErrorOr;
using JualIn.Domain.Catalogs.Entities;
using Mediator;

namespace JualIn.App.Mobile.Core.Modules.Catalogs.Commands.Save
{
    public record SaveCommand(Product Product) : ICommand<ErrorOr<Success>>;
}
