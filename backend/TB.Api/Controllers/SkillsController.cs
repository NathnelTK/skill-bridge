using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TB.Application.Abstractions;
using TB.Application.Skills;

namespace TB.Api.Controllers;

[ApiController]
[Route("api/skills")]
[Authorize]
public sealed class SkillsController(ISkillService skillService) : ControllerBase
{
    [HttpGet]
    [AllowAnonymous]
    [ProducesResponseType(typeof(IReadOnlyList<SkillDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<SkillDto>>> List(CancellationToken ct)
    {
        var skills = await skillService.ListAsync(ct);
        return Ok(skills);
    }
}
