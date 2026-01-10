using ErrorOr;
using JualIn.Domain.Account.Entities;

namespace JualIn.Domain.Account
{
    public interface IAccountValidator
    {
        ErrorOr<Success> ValidateAccount(RegistrationInfo info);
    }
}
