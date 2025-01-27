using IndividualsRegistry.Infrastructure.Models.Configuration;
using Microsoft.Extensions.Options;

namespace IndividualsRegistry.Presentation.Api.Configuration;

public class CultureOptionsSetup : IConfigureOptions<Culture>
{
    private const string _sectionName = nameof(Culture);
    private readonly IConfiguration _configuration;

    public CultureOptionsSetup(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void Configure(Culture options)
    {
        _configuration.GetSection(_sectionName).Bind(options);
    }
}
