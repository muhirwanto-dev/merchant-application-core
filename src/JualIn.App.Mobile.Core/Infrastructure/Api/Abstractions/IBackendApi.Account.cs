using JualIn.Contracts.Dtos.Account;
using Refit;

namespace JualIn.App.Mobile.Core.Infrastructure.Api.Abstractions
{
    public partial interface IBackendApi
    {
        [Get("/v1/user/{userIdentity}/info")]
        public Task<IApiResponse<GetUserInformationResponseDto>> GetUserInformationAsync(string userIdentity, CancellationToken cancellationToken = default);
    }
}
