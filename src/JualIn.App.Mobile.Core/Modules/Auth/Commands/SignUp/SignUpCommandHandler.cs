using ErrorOr;
using JualIn.App.Mobile.Core.Infrastructure.Api.Abstractions;
using JualIn.Contracts.Dtos.Auth.EmailSignUp;
using JualIn.SharedLib.Extensions.Refit;
using Mediator;

namespace JualIn.App.Mobile.Core.Modules.Auth.Commands.SignUp
{
    public class SignUpCommandHandler(
        IBackendApi _api) : ICommandHandler<SignUpCommand, ErrorOr<Success>>
    {
        public async ValueTask<ErrorOr<Success>> Handle(SignUpCommand command, CancellationToken cancellationToken)
        {
            var response = await _api.SignUpAsync(new EmailSignUpRequestDto(
                command.Email,
                command.Password,
                command.FirstName,
                command.LastName,
                command.BusinessName,
                command.BusinessCategory
                ), cancellationToken);

            if (response.IsSuccessful && response.Content is EmailSignUpResponseDto dto)
            {
                // todo: process the dto

                return Result.Success;
            }

            return Error.Failure(description: response.GetMessageOrDefault());
        }
    }
}
