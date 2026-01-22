namespace JualIn.App.Mobile.Presentation.Modules.Auth.Abstractions
{
    public interface ITokenService
    {
        Task<bool> IsExpiredAsync();

        Task<string?> GetAccessTokenAsync();

        Task<string?> GetRefreshTokenAsync();

        Task SaveSignInDataAsync(string accessToken, string? refreshToken, DateTime? expiration, CancellationToken cancellationToken = default);
    }
}
