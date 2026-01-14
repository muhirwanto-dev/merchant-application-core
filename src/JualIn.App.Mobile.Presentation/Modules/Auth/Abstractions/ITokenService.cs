namespace JualIn.App.Mobile.Presentation.Modules.Auth.Abstractions
{
    public interface ITokenService
    {
        bool IsExpired { get; }

        Task<string?> GetAccessTokenAsync();

        Task<string?> GetRefreshTokenAsync();
    }
}
