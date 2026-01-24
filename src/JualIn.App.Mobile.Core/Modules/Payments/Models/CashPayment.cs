using ErrorOr;
using JualIn.App.Mobile.Core.Modules.Payments.Abstractions;

namespace JualIn.App.Mobile.Core.Modules.Payments.Models
{
    public class CashPayment : IPayment
    {
        public Task<ErrorOr<Success>> ExecuteAsync(double amount, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(Result.Success.ToErrorOr());
        }
    }
}
