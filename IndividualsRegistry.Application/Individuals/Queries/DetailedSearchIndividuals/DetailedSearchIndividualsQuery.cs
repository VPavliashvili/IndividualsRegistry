using IndividualsRegistry.Application.Models;
using IndividualsRegistry.Domain.Enums;
using MediatR;

namespace IndividualsRegistry.Application.Individuals.Queries.DetailedSearchIndividuals;

public sealed class DetailedSearchIndividualsQuery : IRequest<List<DetailedSearchIndividualsResponse>>
{
    public required string? Name { get; set; }
    public required string? Surname { get; set; }
    public required Gender? Gender { get; set; }
    public required string? PersonalId { get; set; }
    public required DateOnly? BirthDate { get; set; }
    public required int? CityId { get; set; }
    public required string? PhoneNumber { get; set; }
    public required RelationType? RelationType { get; set; }

    public required int PageSize { get; set; }
    public required int PageNumber { get; set; }
}
