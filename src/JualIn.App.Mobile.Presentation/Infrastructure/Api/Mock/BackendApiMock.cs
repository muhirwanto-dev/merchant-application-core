using JualIn.App.Mobile.Presentation.Configurations;
using Refit;

namespace JualIn.App.Mobile.Presentation.Infrastructure.Api.Mock
{
    public partial class BackendApiMock : IBackendApi
    {
        private IApiResponse<T> CreateApiResponse<T>(T content)
        {
            return new ApiResponse<T>
            (
                response: new HttpResponseMessage(System.Net.HttpStatusCode.OK),
                content: content,
                settings: GlobalRefitSettings.Default
            );
        }
    }
}
