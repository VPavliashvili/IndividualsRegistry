using IndividualsRegistry.Domain.Enums;
using MediatR;

namespace IndividualsRegistry.Application.Individuals.Queries.DetailedSearchIndividuals;

public sealed class DetailedSearchIndividualsQuery
    : IRequest<List<DetailedSearchIndividualsResponse>>
{
    public string? Name { get; set; }
    public string? Surname { get; set; }
    public Gender? Gender { get; set; }
    public string? PersonalId { get; set; }
    public DateOnly? BirthDate { get; set; }
    public int? CityId { get; set; }
    public string? PhoneNumber { get; set; }
    public RelationType? RelationType { get; set; }

    public int PageSize { get; set; }
    public int PageNumber { get; set; }
}
