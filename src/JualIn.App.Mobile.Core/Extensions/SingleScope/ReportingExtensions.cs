using Refit;
using SingleScope.Reporting.Abstractions;
using SingleScope.Reporting.Models;

namespace JualIn.App.Mobile.Core.Extensions.SingleScope
{
    public static partial class ReportingExtensions
    {
        extension(IReportingService reporting)
        {
            public void ReportProblems(Exception exception, string? message = null)
            {
                if (exception is ValidationApiException vae)
                {
                    reporting.Report(new Report(vae, $"{vae.Content?.Title ?? vae.Message}. {message}"));
                }
                else
                {
                    reporting.Report(new Report(exception, message));
                }
            }

            public Task ReportProblemsAsync(Exception exception, string? message = null)
            {
                if (exception is ValidationApiException vae)
                {
                    return reporting.ReportAsync(new Report(vae, $"{vae.Content?.Title ?? vae.Message}. {message}"));
                }
                else
                {
                    return reporting.ReportAsync(new Report(exception, message));
                }
            }
        }
    }
}
