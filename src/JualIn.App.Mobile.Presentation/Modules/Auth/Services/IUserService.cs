namespace JualIn.App.Mobile.Presentation.Modules.Auth.Services
{
    public interface IUserService
    {
        User User { get; }

        bool HasUserData { get; }

        ValueTask FetchUserDataAsync(CancellationToken cancellationToken = default);
    }
}
