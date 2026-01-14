using CommunityToolkit.Diagnostics;
using JualIn.SharedLib.Extensions.Refit;
using Refit;
using SingleScope.Maui.Dialogs;
using SingleScope.Maui.Dialogs.Abstractions;

namespace JualIn.App.Mobile.Presentation.Core.Extensions.SingleScope
{
    public static class DialogServiceExtensions
    {
        extension(IDialogService dialogService)
        {
            public Task ShowInfoOrThrowAsync(IApiResponse response)
            {
                if (response.GetMessage() is string str)
                {
                    return dialogService.ShowAsync(Alert.Info(str));
                }

                throw response.Error ?? ThrowHelper.ThrowInvalidOperationException<ApiException>();
            }
        }
    }
}
