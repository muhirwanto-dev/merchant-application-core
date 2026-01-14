using JualIn.Domain.Common.Messaging;
using JualIn.Domain.Payments.ValueObjects;

namespace JualIn.Domain.Sales.Events
{
    public record OrderPaidEvent(string OrderId, double PaidAmount, PaymentMethod PaymentMethod) : IDomainEvent;
}
