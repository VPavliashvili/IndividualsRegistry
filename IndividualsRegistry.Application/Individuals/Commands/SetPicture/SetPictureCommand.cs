using MediatR;

namespace IndividualsRegistry.Application.Individuals.Commands.SetPicture;

public sealed record SetPictureCommand(int individualId, byte[] image) : IRequest;
