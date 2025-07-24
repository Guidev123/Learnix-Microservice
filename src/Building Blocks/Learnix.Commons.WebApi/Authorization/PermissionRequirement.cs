using Microsoft.AspNetCore.Authorization;

namespace Learnix.Commons.WebApi.Authorization
{
    internal sealed class PermissionRequirement : IAuthorizationRequirement
    {
        public PermissionRequirement(string permission) => Permission = permission;

        public string Permission { get; }
    }
}