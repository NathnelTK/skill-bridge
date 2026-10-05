using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TB.Api.Extensions;
using TB.Application.Abstractions;
using TB.Application.Jobs;

namespace TB.Api.Controllers;

[ApiController]
[Route("api/employer/jobs")]
[Authorize(Roles = "Employer")]
public sealed class EmployerJobsController(IJobService jobService) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<JobSummaryDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<IReadOnlyList<JobSummaryDto>>> List(CancellationToken ct)
    {
        var jobs = await jobService.ListForCurrentEmployerAsync(User.GetUserId(), ct);
        return Ok(jobs);
    }

    [HttpGet("{id:guid}/applicants")]
    [ProducesResponseType(typeof(IReadOnlyList<ApplicantDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IReadOnlyList<ApplicantDto>>> Applicants(Guid id, CancellationToken ct)
    {
        var applicants = await jobService.GetApplicantsAsync(User.GetUserId(), id, ct);
        return Ok(applicants);
    }
}
