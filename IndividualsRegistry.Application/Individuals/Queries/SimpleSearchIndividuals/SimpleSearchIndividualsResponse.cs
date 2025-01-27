using IndividualsRegistry.Application.Models;

namespace IndividualsRegistry.Application.Individuals.Queries.SimpleSearchIndividuals;

public class SimpleSearchIndividualsResponse : Individual
{
    public byte[]? Picture { get; set; }
}

