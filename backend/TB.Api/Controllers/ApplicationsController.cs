using Microsoft.AspNetCore.Mvc;
using TB.Application.Features.Applications;

namespace TB.Api.Controllers;

[ApiController]
[Route("api/applications")]
public sealed class ApplicationsController : ControllerBase
{
    private readonly IApplicationService _applicationService;

    public ApplicationsController(
        IApplicationService applicationService)
    {
        _applicationService = applicationService;
    }

    [HttpPost]
    public async Task<ActionResult<ApplicationDto>> CreateApplication(
        [FromHeader(Name = "X-Candidate-Id")] Guid candidateId,
        [FromBody] CreateApplicationRequest request,
        CancellationToken cancellationToken)
    {
        if (candidateId == Guid.Empty)
        {
            return BadRequest(new
            {
                message = "X-Candidate-Id header is required."
            });
        }

        try
        {
            var application =
                await _applicationService.CreateApplicationAsync(
                    candidateId,
                    request,
                    cancellationToken);

            if (application is null)
            {
                return BadRequest(new
                {
                    message = "Application could not be created."
                });
            }

            return StatusCode(
                StatusCodes.Status201Created,
                application);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new
            {
                message = ex.Message
            });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new
            {
                message = ex.Message
            });
        }
    }

    [HttpGet("candidate/{candidateId:guid}")]
    public async Task<ActionResult<List<CandidateApplicationDto>>>
        GetCandidateApplications(
            Guid candidateId,
            CancellationToken cancellationToken)
    {
        var applications =
            await _applicationService
                .GetCandidateApplicationsAsync(
                    candidateId,
                    cancellationToken);

        return Ok(applications);
    }
}
