using MediatR;

namespace IndividualsRegistry.Application.Individuals.Queries.SimpleSearchIndividuals;

public class SimpleSearchIndividualsQuery : IRequest<List<SimpleSearchIndividualsResponse>>
{
    public string? Name { get; set; }
    public string? Surname { get; set; }
    public string? Personalid { get; set; }
    public int PageSize { get; set; }
    public int PageNumber { get; set; }
}
