using JualIn.App.Mobile.Core.Modules.Auth.Models;

namespace JualIn.App.Mobile.Core.Modules.Auth.Abstractions
{
    public interface IUserService
    {
        User User { get; }

        bool HasUserData { get; }

        ValueTask FetchUserDataAsync(string userIdentity, CancellationToken cancellationToken = default);
    }
}
