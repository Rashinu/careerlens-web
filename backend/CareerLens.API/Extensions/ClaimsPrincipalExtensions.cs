using System.Security.Claims;

namespace CareerLens.API.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static Guid GetUserId(this ClaimsPrincipal principal)
    {
        var claim = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return Guid.TryParse(claim, out var id) ? id : throw new InvalidOperationException("Kullanıcı kimliği bulunamadı.");
    }
}
