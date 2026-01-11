using CommunityToolkit.Diagnostics;
using JualIn.App.Mobile.Presentation.Core.Extensions;
using JualIn.App.Mobile.Presentation.Infrastructure.Api;
using JualIn.App.Mobile.Presentation.Infrastructure.Storages;
using JualIn.Contracts.Dtos.Account;
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

/* Unmerged change from project 'Belibu.App (net9.0-ios)'
Before:
                SecureStorage.Default.SetAsync(Constants.StorageKeys.AccessToken, accessToken),
                SecureStorage.Default.SetAsync(Constants.StorageKeys.RefreshToken, refreshToken ?? string.Empty),
            ]);
After:
                SecureStorage.Default.SetAsync(StorageKeys.AccessToken, accessToken),
                SecureStorage.Default.SetAsync(StorageKeys.RefreshToken, refreshToken ?? string.Empty),
            ]);
*/
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

            var result = await _api.GetUserInformationAsync(_userIdentity);
            var msg = result.GetMessage();

            _logger.LogInformation($"Fetching user data result: {msg}");

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


        /* Unmerged change from project 'Belibu.App (net9.0-ios)'
        Before:
                public Task<string?> GetAccessTokenAsync() => SecureStorage.Default.GetAsync(Constants.StorageKeys.AccessToken);

                public Task<string?> GetRefreshTokenAsync() => SecureStorage.Default.GetAsync(Constants.StorageKeys.RefreshToken);
            }
        After:
                public Task<string?> GetAccessTokenAsync() => SecureStorage.Default.GetAsync(StorageKeys.AccessToken);

                public Task<string?> GetRefreshTokenAsync() => SecureStorage.Default.GetAsync(StorageKeys.RefreshToken);
            }
        */
        public Task<string?> GetAccessTokenAsync() => SecureStorage.Default.GetAsync(StorageKeys.AccessToken);

        public Task<string?> GetRefreshTokenAsync() => SecureStorage.Default.GetAsync(StorageKeys.RefreshToken);
    }
}
