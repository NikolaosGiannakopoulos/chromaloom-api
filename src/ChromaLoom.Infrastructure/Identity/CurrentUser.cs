using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using ChromaLoom.Kernel.Abstractions.Identity;

namespace ChromaLoom.Infrastructure.Identity;

internal sealed class CurrentUser(IHttpContextAccessor httpContextAccessor) : ICurrentUser
{
    public string? Id => httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier)
        ?? httpContextAccessor.HttpContext?.User.FindFirstValue("sub");
}
