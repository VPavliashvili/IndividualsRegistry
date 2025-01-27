namespace IndividualsRegistry.Domain.Exceptions;

public class AlreadyExistsException : Exception
{
    public AlreadyExistsException(int individualId)
        : base()
    {
        IndividualId = individualId;
    }

    public int IndividualId { get; }
}
