using System.Linq.Expressions;
using IndividualsRegistry.Domain.Entities;

namespace IndividualsRegistry.Domain.Specifications;

public interface IIndividualSpecification
{
    public int? PageSize { get; }
    public int? PageNumber { get; }

    Expression<Func<IndividualEntity, bool>> Criteria { get; }
}

