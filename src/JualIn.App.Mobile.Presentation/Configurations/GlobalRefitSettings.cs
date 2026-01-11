using Refit;
using SingleScope.Extensions.Json;

namespace JualIn.App.Mobile.Presentation.Configurations
{
    public class GlobalRefitSettings
    {
        public static readonly RefitSettings Default = new()
        {
            ContentSerializer = new SystemTextJsonContentSerializer(WebJsonSerializerOptions.Default),
            UrlParameterKeyFormatter = new CamelCaseUrlParameterKeyFormatter(),
        };
    }
}
