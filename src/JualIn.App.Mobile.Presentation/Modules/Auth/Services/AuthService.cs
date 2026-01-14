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
        private DateTime? _expiration;
        private User? _user;
        private string _userIdentity = string.Empty;

        public User User => _user ?? User.Default;

        public bool IsExpired => _expiration == null || DateTime.UtcNow >= _expiration;

        public bool HasUserData => _user != null;

        public Task SaveSignInDataAsync(string userIdentity, string accessToken, string? refreshToken, DateTime? expiration,
            CancellationToken cancellationToken = default)
        {
            _expiration = expiration;
            _userIdentity = userIdentity;

            return Task.WhenAll([
                SecureStorage.Default.SetAsync(StorageKeys.AccessToken, accessToken),
                SecureStorage.Default.SetAsync(StorageKeys.RefreshToken, refreshToken ?? string.Empty),
            ]);
        }

        public async ValueTask FetchUserDataAsync(CancellationToken cancellationToken = default)
        {
            if (_user != null && !IsExpired)
            {
                return;
            }

            var result = await _api.GetUserInformationAsync(_userIdentity, cancellationToken);
            var msg = result.GetMessage();

            _logger.LogInformation("Fetching user data result: {msg}", msg);

            if (!result.IsSuccessful)
            {
                ThrowHelper.ThrowInvalidDataException(msg);
            }

            GetUserInformationResponseDto dto = result.Content;

            _user = new User(
                _userIdentity,
                dto.Username,
                dto.Email,
                dto.FirstName,
                dto.FullName
            );
        }

        public Task<string?> GetAccessTokenAsync() => SecureStorage.Default.GetAsync(StorageKeys.AccessToken);

        public Task<string?> GetRefreshTokenAsync() => SecureStorage.Default.GetAsync(StorageKeys.RefreshToken);
    }
}
