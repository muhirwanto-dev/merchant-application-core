using System.Text.Json;
using CommunityToolkit.Diagnostics;
using Refit;
using SingleScope.Extensions.Json;

namespace JualIn.SharedLib.Extensions.Refit
{
    public static class ApiResponseExtensions
    {
        extension(IApiResponse response)
        {
            public ProblemDetails? GetProblem()
            {
                if (response.Error?.Content == null)
                {
                    return null;
                }

                if (response.Error is ValidationApiException vae)
                {
                    return vae.Content;
                }

                try
                {
                    return JsonSerializer.Deserialize<ProblemDetails>(response.Error.Content, WebJsonSerializerOptions.Default);
                }
                catch
                {
                    return null;
                }
            }

            public string? GetMessage()
            {
                string? message = null;

                if (response.GetProblem() is ProblemDetails problem)
                {
                    if (response.StatusCode < System.Net.HttpStatusCode.InternalServerError)
                    {
                        message = problem.Detail;

                        if (problem.Errors.Count > 0)
                        {
                            message ??= string.Join("\n", problem.Errors.First().Value);
                        }
                    }

                    message ??=
                        problem.Title ??
                        problem.Status.ToString();
                }

                message ??=
                    response.Error?.Content ??
                    $"({((int)response.StatusCode)}) {response.ReasonPhrase ?? response.StatusCode.ToString()}";

                return message;
            }

            public string GetMessageOrDefault(string fallback = "Unknown Api Error")
                => response.GetMessage() ?? fallback;

            public string GetMessageOrThrow()
                => response.GetMessage() ?? throw response.Error ?? ThrowHelper.ThrowInvalidOperationException<ApiException>();
        }
    }
}
