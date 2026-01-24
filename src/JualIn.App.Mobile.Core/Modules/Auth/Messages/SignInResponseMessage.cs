using JualIn.App.Mobile.Core.Messaging.Abstractions;

namespace JualIn.App.Mobile.Core.Modules.Auth.Messages
{
    public record SignInResponseMessage(bool IsConfirmed) : IUIMessage;
}
