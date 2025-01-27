namespace IndividualsRegistry.Domain.Exceptions;

public class PageNumberOverflowException : Exception
{
    public int MaxPage { get; }
    public int GivenPage { get; }
    public int FilteredCount { get; }

    public PageNumberOverflowException(int maxPage, int givenPage, int filteredCount)
        : base()
    {
        MaxPage = maxPage;
        GivenPage = givenPage;
        FilteredCount = filteredCount;
    }
}

