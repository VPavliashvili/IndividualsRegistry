using MediatR;

namespace IndividualsRegistry.Application.Individuals.Queries.GetFullIndividualInfo;

public sealed record GetFullIndividualInfoQuery(int IndividualId)
    : IRequest<GetFullIndividualInfoResponse>;
