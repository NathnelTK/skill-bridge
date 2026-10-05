using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TB.Api.Extensions;
using TB.Application.Abstractions;
using TB.Application.Applications;
using TB.Application.Candidates;

namespace TB.Api.Controllers;

[ApiController]
[Route("api/candidates/me")]
[Authorize(Roles = "Candidate")]
public sealed class CandidatesController(
    ICandidateProfileService candidateProfileService,
    IApplicationService applicationService) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(CandidateProfileDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<CandidateProfileDto>> GetProfile(CancellationToken ct)
    {
        var profile = await candidateProfileService.GetCurrentAsync(User.GetUserId(), ct);
        return Ok(profile);
    }

    [HttpPut]
    [ProducesResponseType(typeof(CandidateProfileDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<CandidateProfileDto>> UpdateProfile(
        [FromBody] UpdateCandidateProfileRequest request,
        CancellationToken ct)
    {
        var profile = await candidateProfileService.UpdateCurrentAsync(User.GetUserId(), request, ct);
        return Ok(profile);
    }

    [HttpGet("applications")]
    [ProducesResponseType(typeof(IReadOnlyList<CandidateApplicationDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<IReadOnlyList<CandidateApplicationDto>>> ListApplications(
        CancellationToken ct)
    {
        var applications = await applicationService.ListForCurrentCandidateAsync(User.GetUserId(), ct);
        return Ok(applications);
    }
}
