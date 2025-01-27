using System.Linq.Expressions;
using IndividualsRegistry.Domain.Entities;
using IndividualsRegistry.Domain.Specifications;

namespace IndividualsRegistry.Application.Specifications;

public class SimpleSearchSpec : IIndividualSpecification
{
    public int PageSize { get; }
    public int PageNumber { get; }

    public Expression<Func<IndividualEntity, bool>> Criteria { get; }

    public SimpleSearchSpec(
        int pageSize,
        int pageNumber,
        string? name,
        string? surname,
        string? personalId
    )
    {
        PageSize = pageSize;
        PageNumber = pageNumber;

        Console.WriteLine(name);
        Criteria = (x) =>
            x.Name.Contains(name ?? string.Empty)
            || x.Surname.Contains(surname ?? string.Empty)
            || x.PersonalId.Contains(personalId ?? string.Empty);
    }
}
