using CommunityToolkit.Mvvm.Messaging;
using ErrorOr;
using JualIn.App.Mobile.Core.Infrastructure.Api.Abstractions;
using JualIn.App.Mobile.Core.Modules.Auth.Messages;
using JualIn.Contracts.Dtos.Auth;
using JualIn.SharedLib.Extensions.Refit;
using Mediator;

namespace JualIn.App.Mobile.Core.Modules.Auth.Queries.IsAvailable
{
    public class IsAvailableQueryHandler(
        IBackendApi _api,
        IMessenger _messenger
        ) : IQueryHandler<IsAvailableQuery, ErrorOr<bool>>
    {
        public async ValueTask<ErrorOr<bool>> Handle(IsAvailableQuery query, CancellationToken cancellationToken)
        {
            var response = await _api.CheckAvailabilityAsync(query.Email, cancellationToken);
            if (response.IsSuccessful && response.Content is CheckEmailAvailabilityResponseDto dto)
            {
                _messenger.Send(new EmailAvailabilityMessage(dto.IsAvailable, dto.Message));

                return dto.IsAvailable;
            }

            return Error.Failure(description: response.GetMessageOrDefault());
        }
    }
}
