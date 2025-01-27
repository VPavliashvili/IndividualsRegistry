using IndividualsRegistry.Application.Individuals.Commands.CreateIndividual;
using IndividualsRegistry.Application.Individuals.Commands.EditIndividual;
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

    [HttpGet("individual/{id}", Name = nameof(GetIndividual))]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetIndividual(int id)
    {
        throw new NotImplementedException();
    }
}
