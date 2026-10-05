using System.Security.Claims;

namespace TB.Api.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static Guid GetUserId(this ClaimsPrincipal principal)
    {
        var value = principal.FindFirstValue(ClaimTypes.NameIdentifier);

        if (Guid.TryParse(value, out var userId))
        {
            return userId;
        }

        throw new UnauthorizedAccessException("The authenticated principal is missing a valid user identifier.");
    }
}
