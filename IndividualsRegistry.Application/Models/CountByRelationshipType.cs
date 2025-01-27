using IndividualsRegistry.Domain.Enums;

namespace IndividualsRegistry.Application.Models;

public class CountByRelationshipType
{
    public RelationType Type { get; set; }
    public required int Count { get; set; }
}

