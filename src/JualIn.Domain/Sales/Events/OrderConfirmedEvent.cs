using JualIn.Domain.Common.Messaging;
using JualIn.Domain.Payments.ValueObjects;

namespace JualIn.Domain.Sales.Events
{
    public record OrderConfirmedEvent(string OrderId, PaymentMethod PaymentMethod) : IDomainEvent;
}
