using Learnix.Commons.Application.Exceptions;
using System.Security.Claims;

namespace Learnix.Commons.WebApi.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static Guid GetUserId(this ClaimsPrincipal claimsPrincipal)
        {
            var userId = claimsPrincipal?.FindFirst(CustomClaimsExtensions.Sub)?.Value;
            return Guid.TryParse(userId, out var parsedUserId)
                ? parsedUserId
                : throw new LearnixException("User identifier is unavaible");
        }

        public static string GetIdentityId(this ClaimsPrincipal claimsPrincipal)
        {
            return claimsPrincipal?.FindFirst(ClaimTypes.NameIdentifier)?.Value
                ?? throw new LearnixException("User identity is unavaible");
        }

        public static HashSet<string> GetPermissions(this ClaimsPrincipal claimsPrincipal)
        {
            var permissionClaims = claimsPrincipal?.FindAll(CustomClaimsExtensions.Permission)
                ?? throw new LearnixException("Permissions are unavaible");

            return permissionClaims.Select(c => c.Value).ToHashSet();
        }
    }
}