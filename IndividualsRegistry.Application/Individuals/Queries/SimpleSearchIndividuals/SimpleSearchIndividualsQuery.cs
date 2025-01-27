using MediatR;

namespace IndividualsRegistry.Application.Individuals.Queries.SimpleSearchIndividuals;

public class SimpleSearchIndividualsQuery : IRequest<SimpleSearchIndividualsResponse>
{
    public string? Name { get; set; }
    public string? Surname { get; set; }
    public string? PersonalId;
    public int? PageNumber { get; set; }
    public int? PageSize { get; set; }
}
