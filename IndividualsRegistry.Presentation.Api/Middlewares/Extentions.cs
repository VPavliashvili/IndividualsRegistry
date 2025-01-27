namespace IndividualsRegistry.Presentation.Api.Middlewares;

public static class Extentions
{
    public static IApplicationBuilder UseErrorLogging(this WebApplication app)
    {
        return app.UseMiddleware<ErrorLoggingMiddleware>();
    }

    public static IApplicationBuilder UseCultureReaderMiddleware(this WebApplication app)
    {
        return app.UseMiddleware<CultureReaderMiddleware>();
    }
}
