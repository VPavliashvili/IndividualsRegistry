using System.Linq.Expressions;
using IndividualsRegistry.Domain.Entities;

namespace IndividualsRegistry.Domain.Specifications;

public class IndividualSpecification { }

public interface IIndividualSpecification
{
    public int? PageSize { get; }
    public int? PageNumber { get; }

    // search by name, surname, personalId
    Expression<Func<string, string, string, bool>> SimpleCriteria { get; }

    // detailed search criteria
    Expression<Func<IndividualEntity, bool>> DetailedCriteria { get; }
}
