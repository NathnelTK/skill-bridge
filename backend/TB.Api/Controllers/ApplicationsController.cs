using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TB.Api.Extensions;
using TB.Application.Abstractions;
using TB.Application.Applications;

namespace TB.Api.Controllers;

[ApiController]
[Route("api/applications")]
[Authorize(Roles = "Employer")]
public sealed class ApplicationsController(IApplicationService applicationService) : ControllerBase
{
    [HttpPatch("{id:guid}/status")]
    [ProducesResponseType(typeof(ApplicationDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<ApplicationDto>> UpdateStatus(
        Guid id,
        [FromBody] UpdateApplicationStatusRequest request,
        CancellationToken ct)
    {
        var application = await applicationService.UpdateStatusAsync(User.GetUserId(), id, request, ct);
        return Ok(application);
    }
}
