using JualIn.Domain.Payments.ValueObjects;
using JualIn.Domain.Sales.Entities;
using JualIn.Domain.Sales.ValueObjects;

namespace JualIn.App.Mobile.Presentation.Modules.Sales.Factories
{
    public class OrderTransactionFactory
    {
        public OrderTransaction Create(Order order, double paidAmount, PaymentMethod paymentMethod) => new()
        {
            TransactionId = Guid.NewGuid().ToString(),
            TransactionType = TransactionType.Sale,
            OrderId = order.Id,
            IsConfirmed = false,
            SettledAmount = order.GrandTotal,
            PaidAmount = paidAmount,
            ChangeAmount = paidAmount - order.GrandTotal,
            PaymentMethod = paymentMethod.ToString()
        };
    }
}
