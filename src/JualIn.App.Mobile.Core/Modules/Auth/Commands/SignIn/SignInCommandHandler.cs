using CommunityToolkit.Mvvm.Messaging;
using ErrorOr;
using JualIn.App.Mobile.Core.Infrastructure.Api.Abstractions;
using JualIn.App.Mobile.Core.Modules.Auth.Abstractions;
using JualIn.App.Mobile.Core.Modules.Auth.Messages;
using JualIn.Contracts.Dtos.Auth.EmailSignIn;
using JualIn.SharedLib.Extensions.Refit;
using Mediator;

namespace JualIn.App.Mobile.Core.Modules.Auth.Commands.SignIn
{
    public class SignInCommandHandler(
        IBackendApi _api,
        IMessenger _messenger,
        IAuthService _authService
        ) : ICommandHandler<SignInCommand, ErrorOr<Success>>
    {
        public async ValueTask<ErrorOr<Success>> Handle(SignInCommand command, CancellationToken cancellationToken)
        {
            var response = await _api.SignInAsync(new EmailSignInRequestDto(command.Email, command.Password, command.RememberMe), cancellationToken);
            if (response.IsSuccessful && response.Content is EmailSignInResponseDto dto)
            {
                _messenger.Send(new SignInResponseMessage(dto.IsEmailConfirmed));

                if (dto.IsEmailConfirmed)
                {
                    await Task.WhenAll(
                        _authService.SaveSignInDataAsync(dto.AccessToken, dto.RefreshToken, dto.Expiration, cancellationToken),
                        _authService.FetchUserDataAsync(dto.UserIdentifier, cancellationToken).AsTask()
                        );
                }

                return Result.Success;
            }

            return Error.Failure(description: response.GetMessageOrDefault());
        }
    }
}
