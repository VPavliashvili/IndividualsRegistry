using MediatR;

namespace IndividualsRegistry.Application.Individuals.Commands.SetPicture;

public sealed record SetPictureCommand(SetPictureRequest request) : IRequest<int>;
