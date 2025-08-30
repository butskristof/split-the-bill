using Mediator;
using Microsoft.Extensions.Logging;

namespace SplitTheBill.Application.Common.Pipeline;

// this behavior should wrap the entire Mediator pipeline and apply to all messages passing through: IMessage and *all*
// its derivatives: request, command, query, ...
// it is used to log incoming messages and the time required to get to a response
// logs should be restricted to Debug level to prevent flooding the logs

internal sealed class LoggingBehavior<TMessage, TResponse> : IPipelineBehavior<TMessage, TResponse>
    where TMessage : IMessage
{
    #region construction

    private readonly ILogger<LoggingBehavior<TMessage, TResponse>> _logger;
    private readonly TimeProvider _timeProvider;

    public LoggingBehavior(
        ILogger<LoggingBehavior<TMessage, TResponse>> logger,
        TimeProvider timeProvider
    )
    {
        _logger = logger;
        _timeProvider = timeProvider;
    }

    #endregion

    public async ValueTask<TResponse> Handle(
        TMessage message,
        MessageHandlerDelegate<TMessage, TResponse> next,
        CancellationToken cancellationToken
    )
    {
        var messageTypeName = typeof(TMessage).FullName;
        _logger.LogDebug("Incoming message: {MessageType}", messageTypeName);

        // keep a record of when we start processing the message
        var start = _timeProvider.GetTimestamp();
        try
        {
            // continue in the pipeline
            return await next(message, cancellationToken);
        }
        finally
        {
            // these steps belong in the finally, so even in case an exception occurs, we can still close our logging
            var end = _timeProvider.GetTimestamp(); // stop the timer
            var diff = _timeProvider.GetElapsedTime(start, end); // calculate the time spent
            _logger.LogDebug(
                "Completed processing message of type {MessageType} in {ElapsedMilliseconds} ms",
                messageTypeName,
                diff.TotalMilliseconds
            );
        }
    }
}
