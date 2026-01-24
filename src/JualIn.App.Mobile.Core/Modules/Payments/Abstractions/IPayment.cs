using ErrorOr;

namespace JualIn.App.Mobile.Core.Modules.Payments.Abstractions
{
    public interface IPayment
    {
        Task<ErrorOr<Success>> ExecuteAsync(double amount, CancellationToken cancellationToken = default);
    }
}
