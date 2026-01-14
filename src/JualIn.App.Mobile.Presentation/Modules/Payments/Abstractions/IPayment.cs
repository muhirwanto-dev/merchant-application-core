using ErrorOr;

namespace JualIn.App.Mobile.Presentation.Modules.Payments.Abstractions
{
    public interface IPayment
    {
        Task<ErrorOr<Success>> ExecuteAsync(double amount, CancellationToken cancellationToken = default);
    }
}
