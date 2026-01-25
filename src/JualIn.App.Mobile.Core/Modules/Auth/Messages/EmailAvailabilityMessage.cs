using JualIn.App.Mobile.Core.Messaging.Abstractions;

namespace JualIn.App.Mobile.Core.Modules.Auth.Messages
{
    public record EmailAvailabilityMessage(bool IsAvailable, string? Message) : IUIMessage;
}
