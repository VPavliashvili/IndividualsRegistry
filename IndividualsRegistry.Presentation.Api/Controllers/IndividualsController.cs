using IndividualsRegistry.Application.Individuals.Commands.RemoveRelatedIndividual;
using IndividualsRegistry.Application.Individuals.Commands.AddRelatedIndividual;
using IndividualsRegistry.Application.Individuals.Commands.CreateIndividual;
using IndividualsRegistry.Application.Individuals.Commands.EditIndividual;
using IndividualsRegistry.Application.Individuals.Commands.RemoveIndividual;
using IndividualsRegistry.Application.Individuals.Commands.SetPicture;
using IndividualsRegistry.Application.Individuals.Queries.DetailedSearchIndividuals;
using IndividualsRegistry.Application.Individuals.Queries.SimpleSearchIndividuals;
using IndividualsRegistry.Application.Individuals.Queries.GetFullIndividualInfo;
using IndividualsRegistry.Application.Individuals.Queries.RelatedIndividuals;
using IndividualsRegistry.Domain.Enums;
using IndividualsRegistry.Presentation.Api.Filters;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace IndividualsRegistry.Presentation.Api.Controllers;

[ApiExceptionFilter]
[ApiController]
[Route("api/[controller]")]
public class IndividualsController : ControllerBase
{
    private readonly IMediator _mediator;

    public IndividualsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost()]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateIndividual([FromBody] CreateIndividualRequest request)
    {
        var cmd = new CreateIndividualCommand(request);
        var res = await _mediator.Send(cmd);
        return CreatedAtRoute(nameof(GetIndividual), new { id = res }, res);
    }

    [HttpPatch("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> ModifyIndividualData(
        [FromRoute] int id,
        [FromBody] EditIndividualRequest request
    )
    {
        var cmd = new EditIndividualCommand(id, request);
        await _mediator.Send(cmd);
        return NoContent();
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> RemoveIndividual([FromRoute] int id)
    {
        var cmd = new RemoveIndividualCommand(id);
        await _mediator.Send(cmd);
        return NoContent();
    }

    [HttpPatch("{id}/picture")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> SetPicture([FromRoute] int id, IFormFile image)
    {
        using var memoryStream = new MemoryStream();
        await image.CopyToAsync(memoryStream);
        byte[] pictureData = memoryStream.ToArray();

        var cmd = new SetPictureCommand(id, pictureData);
        await _mediator.Send(cmd);

        return NoContent();
    }

    [HttpPost("{id}/related")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> AddRelatedIndividual(
        int id,
        int individualId,
        RelationType relationType
    )
    {
        var cmd = new AddRelatedIndividualCommand(id, individualId, relationType);
        await _mediator.Send(cmd);

        return NoContent();
    }

    [HttpDelete("{id}/related")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> RemoveRelatedIndividual([FromRoute] int id, int individualId)
    {
        var cmd = new RemoveRelatedIndividualCommand(id, individualId);
        await _mediator.Send(cmd);

        return NoContent();
    }

    [HttpGet("{id}", Name = nameof(GetIndividual))]
    [ProducesResponseType<GetFullIndividualInfoResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetIndividual(int id)
    {
        var cmd = new GetFullIndividualInfoQuery(id);
        var result = await _mediator.Send(cmd);

        return Ok(result);
    }

    [HttpGet("simple")]
    [ProducesResponseType<SimpleSearchIndividualsResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> SimpleFilteredIndividuals(
        [FromQuery] SimpleSearchIndividualsQuery query
    )
    {
        var result = await _mediator.Send(query);

        return Ok(result);
    }

    [HttpGet("detailed")]
    [ProducesResponseType<DetailedSearchIndividualsResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DetailedFilteredIndividuals(
        [FromQuery] DetailedSearchIndividualsQuery query
    )
    {
        var result = await _mediator.Send(query);

        return Ok(result);
    }

    [HttpGet("relations-stats")]
    [ProducesResponseType<RelatedIndividualsResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> RelationsStats()
    {
        var cmd = new RelatedIndividualsQuery();
        var result = await _mediator.Send(cmd);

        return Ok(result);
    }
}
