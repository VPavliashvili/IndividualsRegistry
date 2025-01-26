using IndividualsRegistry.Domain.Enums;

namespace IndividualsRegistry.Domain.Entities;

public class RelationEntity
{
    public required int IndividualId { get; set; }
    public required int RelatedIndividualId { get; set; }
    public required RelationType RelationType { get; set; }
}

