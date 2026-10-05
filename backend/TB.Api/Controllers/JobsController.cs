using Microsoft.AspNetCore.Mvc;
using TB.Application.Features.Jobs;

namespace TB.Api.Controllers;

[ApiController]
[Route("api/jobs")]
public sealed class JobsController : ControllerBase
{
    private readonly IJobService _jobService;

    public JobsController(IJobService jobService)
    {
        _jobService = jobService;
    }

    [HttpGet]
    public async Task<ActionResult<List<JobDto>>> GetJobs(
        [FromQuery] string? skill,
        CancellationToken cancellationToken)
    {
        var jobs = await _jobService.GetJobsAsync(
            skill,
            cancellationToken);

        return Ok(jobs);
    }

    [HttpPost]
    public async Task<ActionResult<JobDto>> CreateJob(
        [FromHeader(Name = "X-Employer-Id")] Guid employerId,
        [FromBody] CreateJobRequest request,
        CancellationToken cancellationToken)
    {
        if (employerId == Guid.Empty)
        {
            return BadRequest(new
            {
                message = "X-Employer-Id header is required."
            });
        }

        try
        {
            var job = await _jobService.CreateJobAsync(
                employerId,
                request,
                cancellationToken);

            if (job is null)
            {
                return NotFound(new
                {
                    message = "Employer not found."
                });
            }

            return StatusCode(
                StatusCodes.Status201Created,
                job);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new
            {
                message = ex.Message
            });
        }
    }
}
