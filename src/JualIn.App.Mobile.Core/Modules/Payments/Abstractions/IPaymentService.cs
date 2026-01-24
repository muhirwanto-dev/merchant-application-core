using ErrorOr;

namespace JualIn.App.Mobile.Core.Modules.Payments.Abstractions
{
    public interface IPaymentService
    {
        void SetPayment(IPayment payment);

        Task<ErrorOr<Success>> ProcessPaymentAsync(double amount, CancellationToken cancellationToken = default);
    }
}
