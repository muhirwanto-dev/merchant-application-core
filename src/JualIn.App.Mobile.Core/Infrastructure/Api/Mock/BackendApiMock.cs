using JualIn.App.Mobile.Core.Configurations;
using JualIn.App.Mobile.Core.Infrastructure.Api.Abstractions;
using Refit;

namespace JualIn.App.Mobile.Core.Infrastructure.Api.Mock
{
    public partial class BackendApiMock : IBackendApi
    {
        private static IApiResponse<T> CreateApiResponse<T>(T content)
            => new ApiResponse<T>(
                response: new HttpResponseMessage(System.Net.HttpStatusCode.OK),
                content: content,
                settings: GlobalRefitSettings.Default
            );
    }
}
