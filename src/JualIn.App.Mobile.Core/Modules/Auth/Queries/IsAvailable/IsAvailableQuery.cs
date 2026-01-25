using ErrorOr;
using Mediator;

namespace JualIn.App.Mobile.Core.Modules.Auth.Queries.IsAvailable
{
    public record IsAvailableQuery(string Email) : IQuery<ErrorOr<bool>>;
}
