using IndividualsRegistry.Application.Models;

namespace IndividualsRegistry.Application.Individuals.Queries.DetailedSearchIndividuals;

public sealed class DetailedSearchIndividualsResponse : Individual
{
    public byte[]? MyProperty { get; set; }
}

