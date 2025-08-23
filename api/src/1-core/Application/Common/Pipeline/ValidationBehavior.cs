using ErrorOr;
using FluentValidation;
using Mediator;
using Microsoft.Extensions.Logging;

namespace SplitTheBill.Application.Common.Pipeline;

// this behavior will retrieve all applicable validators from the DI container and verify whether the incoming
// message passes all of them
// in case it doesn't, the pipeline execution will be cut short and an error result will be returned

// keep in mind the validators registered with the FluentValidation library should mainly do input validation
// and not be concerned with business logic
// effectively, this behavior is intended to stop unprocessable messages from continuing further into the pipeline
// "an *invalid* message should never reach the handler"

internal sealed class ValidationBehavior<TMessage, TResponse>
    : IPipelineBehavior<TMessage, TResponse>
    where TMessage : IMessage
    where TResponse : IErrorOr
{
    #region construction

    private readonly ILogger<ValidationBehavior<TMessage, TResponse>> _logger;
    private readonly IEnumerable<IValidator<TMessage>> _validators;

    public ValidationBehavior(
        ILogger<ValidationBehavior<TMessage, TResponse>> logger,
        IEnumerable<IValidator<TMessage>> validators
    )
    {
        _logger = logger;
        _validators = validators;
    }

    #endregion

    public async ValueTask<TResponse> Handle(
        TMessage message,
        MessageHandlerDelegate<TMessage, TResponse> next,
        CancellationToken cancellationToken
    )
    {
        var messageTypeName = typeof(TMessage).FullName;
        _logger.LogDebug("Validating incoming message of type {MessageType}", messageTypeName);

        if (_validators.Any())
        {
            _logger.LogDebug("Applying {Count} validators for message type", _validators.Count());

            // create a new validation context scoped to this execution
            var context = new ValidationContext<TMessage>(message);
            // run all validators asynchronously and collect the results
            var results = await Task.WhenAll(
                _validators.Select(v => v.ValidateAsync(context, cancellationToken))
            );
            // aggregate failures from all validators into a single list
            var failures = results
                .Where(result => !result.IsValid)
                .SelectMany(result => result.Errors)
                .ToList();

            if (failures.Count > 0)
            {
                // in case validation resulted in failures, return a result with the failures as errors
                // an exception should *not* be thrown: we're doing flow control here, and validation failures
                // are kind of expected (hence, not exceptional)
                _logger.LogDebug("Validation resulted in {Count} failures", failures.Count);

                // map the validation failures to validation errors, preserving the property and message
                var errors = failures.ConvertAll(f =>
                    Error.Validation(f.PropertyName, f.ErrorMessage)
                );

                // use a dynamic cast, followed by casting to the response type
                // this feels wrong, but otherwise reflection has to be used to call TResponse.From(errors)
                return (dynamic)errors;
            }

            // validation passed, continue in the pipeline as usual
            _logger.LogDebug("Validation passed successfully");
        }
        else
        {
            // in case no validators are found for the message type, just return out of the behaviour and
            // continue the pipeline
            _logger.LogDebug(
                "No validators are available for message of type {MessageType}",
                messageTypeName
            );
        }

        return await next(message, cancellationToken);
    }
}
