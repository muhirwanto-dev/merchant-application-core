using ErrorOr;
using JualIn.Contracts.Enums;
using Mediator;

namespace JualIn.App.Mobile.Core.Modules.Auth.Commands.SignUp
{
    public record SignUpCommand(
        string Email,
        string Password,
        string FirstName,
        string? LastName,
        string? BusinessName,
        BusinessCategory? BusinessCategory
        ) : ICommand<ErrorOr<Success>>;
}
