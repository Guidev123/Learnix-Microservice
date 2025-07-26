using Dapper;
using Learnix.Commons.Application.Authorization;
using Learnix.Commons.Application.Factories;
using Learnix.Commons.Application.Messaging;
using Learnix.Commons.Domain.Results;
using Users.Domain.Errors;

namespace Users.Application.Users.UseCases.GetPermissions
{
    internal sealed class GetUserPermissionsQueryHandler(ISqlConnectionFactory sqlConnectionFactory) : IQueryHandler<GetUserPermissionsQuery, PermissionResponse>
    {
        public async Task<Result<PermissionResponse>> ExecuteAsync(GetUserPermissionsQuery request, CancellationToken cancellationToken = default)
        {
            using var connection = sqlConnectionFactory.Create();

            const string sql = $"""
                SELECT DISTINCT
                    u.Id,
                    rp.PermissionCode
                FROM users.Users u
                JOIN users.UserRoles ur ON ur.UserId = u.Id
                JOIN users.RolePermissions rp ON rp.RoleName = ur.RoleName
                WHERE u.IdentityId = @IdentityId
                """;

            var permissions = (await connection.QueryAsync<UserPermission>(sql, new { IdentityId = request.IdentityId })).AsList();

            if (permissions.Count == 0)
            {
                return Result.Failure<PermissionResponse>(UserErrors.PermissionNotFoundForIdenityId(request.IdentityId));
            }

            return new PermissionResponse(permissions[0].Id, permissions.Select(c => c.PermissionCode).ToHashSet());
        }
    }
}