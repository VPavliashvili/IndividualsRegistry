using IndividualsRegistry.Application.Contracts;
using MediatR;

namespace IndividualsRegistry.Application.Individuals.Queries.RelatedIndividuals;

public sealed class RelatedIndividualsHandler
    : IRequestHandler<RelatedIndividualsQuery, IEnumerable<RelatedIndividualsResponse>>
{
    private readonly IIndividualsRepository _repository;

    public RelatedIndividualsHandler(IIndividualsRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<RelatedIndividualsResponse>> Handle(
        RelatedIndividualsQuery request,
        CancellationToken cancellationToken
    )
    {
        var allIndividual = await _repository.GetIndividuals();
        var result = allIndividual.Select(x => new RelatedIndividualsResponse
        {
            Id = x.Id,
            Name = x.Name,
            Surname = x.Surname,
            RelationCounts = _repository.GroupRelationsByTypeAndIndividual(x.Id).Result,
        });

        return result;
    }
}
