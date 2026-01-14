using ErrorOr;
using JualIn.App.Mobile.Presentation.Modules.Payments.Abstractions;

namespace JualIn.App.Mobile.Presentation.Modules.Payments.Services
{
    public class PaymentService : IPaymentService
    {
        private IPayment _payment = default!;

        public Task<ErrorOr<Success>> ProcessPaymentAsync(double amount, CancellationToken cancellationToken = default)
        {
            return _payment.ExecuteAsync(amount, cancellationToken);
        }

        public void SetPayment(IPayment payment)
        {
            _payment = payment;
        }
    }
}
