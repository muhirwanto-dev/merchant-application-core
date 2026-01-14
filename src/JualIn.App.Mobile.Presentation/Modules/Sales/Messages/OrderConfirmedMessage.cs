using JualIn.App.Mobile.Presentation.Core.Messaging;
using JualIn.Domain.Payments.ValueObjects;

namespace JualIn.App.Mobile.Presentation.Modules.Sales.Messages
{
    public record OrderConfirmedMessage(string OrderId, PaymentMethod PaymentMethod) : IUIMessage;
}
