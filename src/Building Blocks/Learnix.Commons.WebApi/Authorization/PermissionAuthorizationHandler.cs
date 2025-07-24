using Learnix.Commons.WebApi.Extensions;
using Microsoft.AspNetCore.Authorization;

namespace Learnix.Commons.WebApi.Authorization
{
    internal sealed class PermissionAuthorizationHandler : AuthorizationHandler<PermissionRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
        {
            var permissions = context.User.GetPermissions();

            if (permissions.Contains(requirement.Permission))
                context.Succeed(requirement);

            return Task.CompletedTask;
        }
    }
}