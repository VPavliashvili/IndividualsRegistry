using IndividualsRegistry.Application.Individuals.Commands.RemoveRelatedIndividual;
using IndividualsRegistry.Application.Individuals.Commands.AddRelatedIndividual;
using IndividualsRegistry.Application.Individuals.Commands.CreateIndividual;
using IndividualsRegistry.Application.Individuals.Commands.EditIndividual;
using IndividualsRegistry.Application.Individuals.Commands.RemoveIndividual;
using IndividualsRegistry.Application.Individuals.Commands.SetPicture;
using IndividualsRegistry.Domain.Enums;
using IndividualsRegistry.Presentation.Api.Filters;
using MediatR;
using MediatR.Wrappers;
using Microsoft.AspNetCore.Mvc;

namespace IndividualsRegistry.Presentation.Api.Controllers;

[ApiExceptionFilter]
[ApiController]
[Route("[controller]")]
public class IndividualsController : ControllerBase
{
    private readonly IMediator _mediator;

    public IndividualsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("individual")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateIndividual([FromBody] CreateIndividualRequest request)
    {
        var cmd = new CreateIndividualCommand(request);
        var res = await _mediator.Send(cmd);
        return CreatedAtRoute(nameof(GetIndividual), new { id = res }, res);
    }

    [HttpPatch("individual")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> ModifyIndividualData(
        int individualId,
        [FromBody] EditIndividualRequest request
    )
    {
        var cmd = new EditIndividualCommand(individualId, request);
        await _mediator.Send(cmd);
        return NoContent();
    }

    [HttpDelete("individual")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> RemoveIndividual(int individualId)
    {
        var cmd = new RemoveIndividualCommand(individualId);
        await _mediator.Send(cmd);
        return NoContent();
    }

    [HttpPatch("picture")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> SetPicture(int individualId, IFormFile image)
    {
        using var memoryStream = new MemoryStream();
        await image.CopyToAsync(memoryStream);
        byte[] pictureData = memoryStream.ToArray();

        var cmd = new SetPictureCommand(individualId, pictureData);
        await _mediator.Send(cmd);

        return NoContent();
    }

    [HttpPost("individual/related")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> AddRelatedIndividual(
        int individualId,
        int relatedIndividualId,
        RelationType relationType
    )
    {
        var cmd = new AddRelatedIndividualCommand(individualId, relatedIndividualId, relationType);
        await _mediator.Send(cmd);

        return NoContent();
    }

    [HttpDelete("individual/related")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> RemoveRelatedIndividual(
        int individualId,
        int relatedIndividualId
    )
    {
        var cmd = new RemoveRelatedIndividualCommand(individualId, relatedIndividualId);
        await _mediator.Send(cmd);

        return NoContent();
    }

    [HttpGet("individual/{id}", Name = nameof(GetIndividual))]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetIndividual(int id)
    {
        throw new NotImplementedException();
    }
}
