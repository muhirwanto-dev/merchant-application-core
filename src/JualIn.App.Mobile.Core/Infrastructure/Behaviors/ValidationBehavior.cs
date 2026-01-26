using ErrorOr;
using FluentValidation;
using Mediator;

namespace JualIn.App.Mobile.Core.Infrastructure.Behaviors
{
    public class ValidationBehavior<TRequest, TResponse>(IValidator<TRequest>? _validator = null)
        : IPipelineBehavior<TRequest, TResponse>
            where TRequest : IRequest<TResponse>
            where TResponse : IErrorOr
    {
        public async ValueTask<TResponse> Handle(TRequest message, MessageHandlerDelegate<TRequest, TResponse> next, CancellationToken cancellationToken)
        {
            if (_validator is null)
            {
                return await next(message, cancellationToken);
            }

            var validationResult = await _validator.ValidateAsync(message, cancellationToken);
            if (validationResult.IsValid)
            {
                return await next(message, cancellationToken);
            }

            var errors = validationResult.Errors
                .ConvertAll(error => Error.Validation(
                    code: error.PropertyName,
                    description: error.ErrorMessage));

            return (dynamic)errors;
        }
    }
}
