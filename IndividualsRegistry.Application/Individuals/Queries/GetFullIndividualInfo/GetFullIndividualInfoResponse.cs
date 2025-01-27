using IndividualsRegistry.Application.Models;

namespace IndividualsRegistry.Application.Individuals.Queries.GetFullIndividualInfo;

public sealed class GetFullIndividualInfoResponse : Individual
{
    public byte[]? Picture { get; set; }
}

