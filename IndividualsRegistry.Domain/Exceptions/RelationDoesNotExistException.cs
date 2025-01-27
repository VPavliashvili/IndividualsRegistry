namespace IndividualsRegistry.Domain.Exceptions;

public class RelationDoesNotExistException : DoesNotExistException
{
    public RelationDoesNotExistException(int individualId, int relatedIndividualId)
        : base(individualId)
    {
        RelatedIndividualId = relatedIndividualId;
    }

    public int RelatedIndividualId { get; }
}

