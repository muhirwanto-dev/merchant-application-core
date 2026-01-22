using JualIn.App.Mobile.Presentation.Modules.Auth.Models;

namespace JualIn.App.Mobile.Presentation.Modules.Auth.Abstractions
{
    public interface IUserService
    {
        User User { get; }

        bool HasUserData { get; }

        ValueTask FetchUserDataAsync(string userIdentity, CancellationToken cancellationToken = default);
    }
}
