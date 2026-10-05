using Microsoft.AspNetCore.Mvc;
using TB.Application.Features.Applicants;
using TB.Application.Features.Applicants.Models;

namespace TB.Api.Controllers;

[ApiController]
[Route("api/candidates")]
public sealed class CandidateProfilesController : ControllerBase
{
    private readonly ICandidateProfileService _candidateProfileService;

    public CandidateProfilesController(
        ICandidateProfileService candidateProfileService)
    {
        _candidateProfileService = candidateProfileService;
    }

    [HttpGet("{candidateId:guid}/profile")]
    public async Task<ActionResult<CandidateProfileDto>> GetProfile(
        Guid candidateId,
        CancellationToken cancellationToken)
    {
        var profile = await _candidateProfileService.GetProfileAsync(
            candidateId,
            cancellationToken);

        if (profile is null)
        {
            return NotFound(new
            {
                message = "Candidate profile not found."
            });
        }

        return Ok(profile);
    }

    [HttpPut("{candidateId:guid}/profile")]
    public async Task<ActionResult<CandidateProfileDto>> UpdateProfile(
        Guid candidateId,
        UpdateCandidateProfileRequest request,
        CancellationToken cancellationToken)
    {
        var profile = await _candidateProfileService.UpdateProfileAsync(
            candidateId,
            request,
            cancellationToken);

        if (profile is null)
        {
            return NotFound(new
            {
                message = "Candidate profile not found."
            });
        }

        return Ok(profile);
    }
}
