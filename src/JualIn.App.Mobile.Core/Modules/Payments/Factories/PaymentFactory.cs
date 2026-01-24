using JualIn.App.Mobile.Core.Modules.Payments.Abstractions;
using JualIn.App.Mobile.Core.Modules.Payments.Models;
using JualIn.Domain.Payments.ValueObjects;
using Microsoft.Extensions.DependencyInjection;

namespace JualIn.App.Mobile.Core.Modules.Payments.Factories
{
    public class PaymentFactory(IServiceProvider _serviceProvider)
    {
        public IPayment CreatePayment(PaymentMethod paymentMethod) => paymentMethod.Name switch
        {
            nameof(PaymentMethod.Cash) => _serviceProvider.GetRequiredService<CashPayment>(),
            _ => throw new NotImplementedException($"Payment method '{paymentMethod}' is not implemented.")
        };
    }
}
