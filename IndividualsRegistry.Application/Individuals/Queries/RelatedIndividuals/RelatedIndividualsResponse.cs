using IndividualsRegistry.Application.Models;

namespace IndividualsRegistry.Application.Individuals.Queries.RelatedIndividuals;

public sealed class RelatedIndividualsResponse
{
    public required int Id { get; set; }
    public required string Name { get; set; }
    public required string Surname { get; set; }
    public Dictionary<string, int> RelationCounts { get; set; } = [];
}
