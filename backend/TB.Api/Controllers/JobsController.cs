using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TB.Api.Extensions;
using TB.Application.Abstractions;
using TB.Application.Jobs;

namespace TB.Api.Controllers;

[ApiController]
[Route("api/jobs")]
[Authorize]
public sealed class JobsController(
    IJobService jobService,
    IApplicationService applicationService) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<JobSummaryDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<JobSummaryDto>>> Browse(
        [FromQuery] Guid[]? skillId,
        CancellationToken ct)
    {
        var jobs = await jobService.BrowseAsync(skillId ?? [], ct);
        return Ok(jobs);
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(JobDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<JobDto>> Get(Guid id, CancellationToken ct)
    {
        var job = await jobService.GetAsync(id, ct);
        return Ok(job);
    }

    [HttpPost]
    [Authorize(Roles = "Employer")]
    [ProducesResponseType(typeof(JobDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<JobDto>> Create([FromBody] CreateJobRequest request, CancellationToken ct)
    {
        var job = await jobService.CreateAsync(User.GetUserId(), request, ct);
        return CreatedAtAction(nameof(Get), new { id = job.Id }, job);
    }

    [HttpPut("{id:guid}")]
    [Authorize(Roles = "Employer")]
    [ProducesResponseType(typeof(JobDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<JobDto>> Update(
        Guid id,
        [FromBody] UpdateJobRequest request,
        CancellationToken ct)
    {
        var job = await jobService.UpdateAsync(User.GetUserId(), id, request, ct);
        return Ok(job);
    }

    [HttpDelete("{id:guid}")]
    [Authorize(Roles = "Employer")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        await jobService.DeleteAsync(User.GetUserId(), id, ct);
        return NoContent();
    }

    [HttpPost("{id:guid}/applications")]
    [Authorize(Roles = "Candidate")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> Apply(Guid id, CancellationToken ct)
    {
        var application = await applicationService.ApplyAsync(User.GetUserId(), id, ct);
        return StatusCode(StatusCodes.Status201Created, application);
    }
}
