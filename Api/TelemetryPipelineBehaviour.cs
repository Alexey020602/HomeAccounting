using System.Diagnostics;
using Mediator;

namespace Api;

public sealed class TelemetryPipelineBehaviour<TMessage, TResponse>(ActivitySource activitySource, ILogger<TelemetryPipelineBehaviour<TMessage, TResponse>> logger): IPipelineBehavior<TMessage, TResponse> where TMessage: IMessage
{
    public async ValueTask<TResponse> Handle(TMessage message, MessageHandlerDelegate<TMessage, TResponse> next, CancellationToken cancellationToken)
    {
        using var activity = activitySource.StartActivity();
        logger.LogInformation("Handling message {@Message}", message);
        try
        {
            var response = await next(message, cancellationToken);
            activity?.SetStatus(ActivityStatusCode.Ok);
            logger.LogInformation("Handled message {@Message} with response {Response}", message, response);
            return response;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error handling message {@Message}", message);
            activity?.SetStatus(ActivityStatusCode.Error, ex.Message);
            activity?.AddException(ex);
            throw;
        }
    }
}