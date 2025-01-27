namespace IndividualsRegistry.Domain.Exceptions;

public class RelatedIndividualAlreadyExists : AlreadyExistsException
{
    public RelatedIndividualAlreadyExists(int individualId, int relatedIndividualId)
        : base(individualId)
    {
        RelatedIndividualId = relatedIndividualId;
    }

    public int RelatedIndividualId { get; }
}

