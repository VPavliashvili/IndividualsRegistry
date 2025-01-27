using Microsoft.AspNetCore.Localization;

namespace IndividualsRegistry.Presentation.Api.Middlewares;

public class ErrorLoggingMiddleware : IMiddleware
{
    private readonly ILogger<ErrorLoggingMiddleware> _logger;

    public ErrorLoggingMiddleware(ILogger<ErrorLoggingMiddleware> logger)
    {
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Unhandled exception occurred while processing " + "{Path}. Error: {Error}",
                context.Request.Path,
                ex.Message
            );

            throw;
        }
    }
}
