using JualIn.Contracts.Dtos.Account;
using Refit;

namespace JualIn.App.Mobile.Presentation.Infrastructure.Api.Mock
{
    public partial class BackendApiMock
    {
        public Task<IApiResponse<GetUserInformationResponseDto>> GetUserInformationAsync(string userIdentity, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(CreateApiResponse(new GetUserInformationResponseDto(
                Username: "offline.user",
                Email: "offline.user@mail.com",
                FirstName: "Yasuar",
                FullName: "Yasuar Aji Naputro"
                )));
        }
    }
}
