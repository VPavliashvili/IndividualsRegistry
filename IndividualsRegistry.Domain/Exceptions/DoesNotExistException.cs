namespace IndividualsRegistry.Domain.Exceptions;

public class DoesNotExistException : Exception
{
    public DoesNotExistException(int individualId)
        : base()
    {
        IndividualId = individualId;
    }

    public int IndividualId { get; }
}
