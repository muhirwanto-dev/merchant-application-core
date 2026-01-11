namespace JualIn.App.Mobile.Presentation.Modules.Auth.Services
{
    public interface ITokenService
    {
        bool IsExpired { get; }

        Task<string?> GetAccessTokenAsync();

        Task<string?> GetRefreshTokenAsync();
    }
}
