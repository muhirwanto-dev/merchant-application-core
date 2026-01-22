using CommunityToolkit.Diagnostics;
using JualIn.App.Mobile.Presentation.Infrastructure.Api;
using JualIn.App.Mobile.Presentation.Modules.Auth.Abstractions;
using JualIn.App.Mobile.Presentation.Modules.Auth.Models;
using JualIn.App.Mobile.Presentation.Shared.Constants;
using JualIn.Contracts.Dtos.Account;
using JualIn.SharedLib.Extensions.Refit;
using Microsoft.Extensions.Logging;

namespace JualIn.App.Mobile.Presentation.Modules.Auth.Services
{
    public class AuthService(
        ILogger<AuthService> _logger,
        IBackendApi _api
        ) : IAuthService
    {
        private User? _user;

        public User User => _user ?? User.Default;

        public bool HasUserData => _user != null;

        public Task SaveSignInDataAsync(string accessToken, string? refreshToken, DateTime? expiration,
            CancellationToken cancellationToken = default)
        {
            return Task.WhenAll([
                SecureStorage.Default.SetAsync(StorageKeys.AccessToken, accessToken),
                SecureStorage.Default.SetAsync(StorageKeys.RefreshToken, refreshToken ?? string.Empty),
                SecureStorage.Default.SetAsync(StorageKeys.UserExpiration, expiration?.ToString() ?? string.Empty),
            ]);
        }

        public async ValueTask FetchUserDataAsync(string userIdentity, CancellationToken cancellationToken = default)
        {
            if (_user != null)
            {
                return;
            }

            var result = await _api.GetUserInformationAsync(userIdentity, cancellationToken);
            var msg = result.GetMessage();

            _logger.LogInformation("Fetching user data result: {msg}", msg);

            if (!result.IsSuccessful)
            {
                ThrowHelper.ThrowInvalidDataException(msg);
            }

            GetUserInformationResponseDto dto = result.Content;

            _user = new User(
                userIdentity,
                dto.Username,
                dto.Email,
                dto.FirstName,
                dto.FullName
            );
        }

        public Task<string?> GetAccessTokenAsync() => SecureStorage.Default.GetAsync(StorageKeys.AccessToken);

        public Task<string?> GetRefreshTokenAsync() => SecureStorage.Default.GetAsync(StorageKeys.RefreshToken);

        public async Task<bool> IsExpiredAsync() => await SecureStorage.Default.GetAsync(StorageKeys.UserExpiration) is not string expirationStr || DateTime.UtcNow >= DateTime.Parse(expirationStr);
    }
}
