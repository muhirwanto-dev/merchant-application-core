using ErrorOr;
using Mediator;

namespace JualIn.App.Mobile.Core.Modules.Auth.Commands.SignIn
{
    public record SignInCommand(string Email, string Password, bool RememberMe) : ICommand<ErrorOr<Success>>;
}
