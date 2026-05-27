using System.Security.Claims;

namespace TierLab.Api.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static Guid? GetSupabaseUserId(this ClaimsPrincipal principal)
    {
        var claimValue = principal.FindFirst("sub")?.Value
                         ?? principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        return Guid.TryParse(claimValue, out var id) ? id : null;
    }

    public static string? GetSupabaseEmail(this ClaimsPrincipal principal)
        => principal.FindFirst("email")?.Value;

    public static string? GetSupabaseUsername(this ClaimsPrincipal principal)
        => principal.FindFirst("name")?.Value;

    public static string? GetSupabasePicture(this ClaimsPrincipal principal)
        => principal.FindFirst("picture")?.Value;
}
