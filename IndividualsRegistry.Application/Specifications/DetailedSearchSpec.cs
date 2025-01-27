using System.Linq.Expressions;
using IndividualsRegistry.Domain.Entities;
using IndividualsRegistry.Domain.Enums;
using IndividualsRegistry.Domain.Specifications;

namespace IndividualsRegistry.Application.Specifications;

public class DetailedSearchSpec : IIndividualSpecification
{
    public int PageSize { get; }

    public int PageNumber { get; }

    public Expression<Func<IndividualEntity, bool>> Criteria { get; }

    public DetailedSearchSpec(
        int pageSize,
        int pageNumber,
        string? name,
        string? surname,
        string? personalId,
        Gender? gender,
        DateOnly? birthDate,
        int? cityId,
        string? phone,
        RelationType? relationType,
        IEnumerable<RelationEntity> relations
    )
    {
        PageSize = pageSize;
        PageNumber = pageNumber;

        Criteria = (x) =>
            x.Name == name
            || x.Surname == surname
            || x.PersonalId == personalId
            || x.Gender == gender
            || x.BirthDate == birthDate
            || x.CityId == cityId
            || x.PhoneNumbers.Any(p => p.Number == phone)
            || relations.Any(p => p.RelationType == relationType);
    }
}
