using System.Globalization;
using IndividualsRegistry.Infrastructure.Models.Configuration;
using Microsoft.Extensions.Options;

namespace IndividualsRegistry.Presentation.Api.Middlewares;

public class CultureReaderMiddleware : IMiddleware
{
    private readonly Culture _cultures;

    public CultureReaderMiddleware(IOptions<Culture> options)
    {
        _cultures = options.Value;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        var acceptLanguageHeader = context.Request.Headers.AcceptLanguage.ToString();
        if (string.IsNullOrEmpty(acceptLanguageHeader))
            return;

        var languages = acceptLanguageHeader
            .Split(',')
            .Select(x => x.Split(';').First().Trim())
            .ToList();

        var culture = languages.FirstOrDefault(lang =>
            _cultures.Supported.Any(supported =>
                supported.StartsWith(lang, StringComparison.OrdinalIgnoreCase)
            )
        );

        if (string.IsNullOrEmpty(culture))
            return;

        var cultureInfo = new CultureInfo(culture);
        CultureInfo.CurrentCulture = cultureInfo;
        CultureInfo.CurrentUICulture = cultureInfo;

        // Set the culture for the current request
        context.Items["Culture"] = culture;

        await next(context);
    }
}

