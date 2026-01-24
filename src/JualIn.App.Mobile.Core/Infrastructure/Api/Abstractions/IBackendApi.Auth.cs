using JualIn.Contracts.Dtos.Auth;
using JualIn.Contracts.Dtos.Auth.EmailSignIn;
using JualIn.Contracts.Dtos.Auth.EmailSignUp;
using Refit;

namespace JualIn.App.Mobile.Core.Infrastructure.Api.Abstractions
{
    public partial interface IBackendApi
    {
        [Get("/v1/account/{email}/available")]
        public Task<IApiResponse<CheckEmailAvailabilityResponseDto>> CheckAvailabilityAsync([AliasAs("email")] string email, CancellationToken cancellationToken = default);

        [Post("/v1/account/signin")]
        public Task<IApiResponse<EmailSignInResponseDto>> SignInAsync(EmailSignInRequestDto payload, CancellationToken cancellationToken = default);

        [Post("/v1/account/signup")]
        public Task<IApiResponse<EmailSignUpResponseDto>> SignUpAsync(EmailSignUpRequestDto payload, CancellationToken cancellationToken = default);
    }
}
