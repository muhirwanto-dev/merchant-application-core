namespace JualIn.App.Mobile.Core.Modules.Auth.Abstractions
{
    public interface ITokenService
    {
        Task<bool> IsExpiredAsync();

        Task<string?> GetAccessTokenAsync();

        Task<string?> GetRefreshTokenAsync();

        Task SaveSignInDataAsync(string accessToken, string? refreshToken, DateTime? expiration, CancellationToken cancellationToken = default);
    }
}
