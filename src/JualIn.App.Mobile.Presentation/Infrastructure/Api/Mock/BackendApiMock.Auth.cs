using JualIn.Contracts.Dtos.Auth;
using JualIn.Contracts.Dtos.Auth.EmailSignIn;
using JualIn.Contracts.Dtos.Auth.EmailSignUp;
using Refit;

namespace JualIn.App.Mobile.Presentation.Infrastructure.Api.Mock
{
    public partial class BackendApiMock
    {
        private readonly IDictionary<string, string> _uid = new Dictionary<string, string>
        {
            { "access_token", Guid.NewGuid().ToString() },
            { "refresh_token", Guid.NewGuid().ToString() },
            { "user_identifier", Guid.NewGuid().ToString() },
        };

        public Task<IApiResponse<CheckEmailAvailabilityResponseDto>> CheckAvailabilityAsync([AliasAs("email")] string email, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(CreateApiResponse(new CheckEmailAvailabilityResponseDto(
                IsAvailable: true,
                Message: "Email is available for registration."
                )));
        }

        public Task<IApiResponse<EmailSignInResponseDto>> SignInAsync(EmailSignInRequestDto payload, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(CreateApiResponse(new EmailSignInResponseDto(
                AccessToken: _uid["access_token"],
                RefreshToken: _uid["refresh_token"],
                Expiration: DateTime.MaxValue,
                UserIdentifier: _uid["user_identifier"],
                IsEmailConfirmed: true
                )));
        }

        public Task<IApiResponse<EmailSignUpResponseDto>> SignUpAsync(EmailSignUpRequestDto payload, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(CreateApiResponse(new EmailSignUpResponseDto(
                AccessToken: _uid["access_token"],
                RefreshToken: _uid["refresh_token"],
                Expiration: DateTime.MaxValue,
                UserIdentifier: _uid["user_identifier"],
                IsEmailConfirmed: true
                )));
        }
    }
}
