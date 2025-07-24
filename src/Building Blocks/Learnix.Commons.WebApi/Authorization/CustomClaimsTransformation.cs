using Learnix.Commons.Application.Authorization;
using Learnix.Commons.Application.Exceptions;
using Learnix.Commons.WebApi.Extensions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Claims;

namespace Learnix.Commons.WebApi.Authorization
{
    internal sealed class CustomClaimsTransformation(IServiceScopeFactory serviceScopeFactory) : IClaimsTransformation
    {
        public async Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
        {
            if (principal.HasClaim(c => c.Type == CustomClaimsExtensions.Sub))
                return principal;

            using var scope = serviceScopeFactory.CreateScope();

            var permissionService = scope.ServiceProvider.GetRequiredService<IPermissionService>();

            var identityId = principal.GetIdentityId();

            var result = await permissionService.GetUserPermissionsAsync(identityId);

            if (result.IsFailure)
                throw new LearnixException(nameof(IPermissionService.GetUserPermissionsAsync), result.Error);

            var claimsIdentity = new ClaimsIdentity();

            claimsIdentity.AddClaim(new(CustomClaimsExtensions.Sub, result.Value.UserId.ToString()));

            foreach (var permission in result.Value.Permissions)
            {
                claimsIdentity.AddClaim(new(CustomClaimsExtensions.Permission, permission));
            }

            principal.AddIdentity(claimsIdentity);

            return principal;
        }
    }
}