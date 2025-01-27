using IndividualsRegistry.Application.Models;

namespace IndividualsRegistry.Application.Individuals.Queries.RelatedIndividuals;

public sealed class RelatedIndividualsResponse
{
    public required IEnumerable<CountByRelationshipType> RelationshipCounts { get; set; }
}

