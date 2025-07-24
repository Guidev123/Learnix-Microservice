using Learnix.Commons.Domain.Results;
using Microsoft.Extensions.Logging;
using System.Net;
using Users.Application.Abstractions.Identity;

namespace Users.Infrastructure.Identity
{
    internal sealed class IdentityProviderService(KeyCloakClient keyCloakClient, ILogger<IdentityProviderService> logger) : IIdentityProviderService
    {
        private const string CredentialType = "password";

        public async Task<Result<string>> RegisterAsync(UserRequest request, CancellationToken cancellationToken = default)
        {
            try
            {
                var identityId = await keyCloakClient.RegisterAsync(new(
                    request.Email,
                    request.Email,
                    request.FirstName,
                    request.LastName,
                    true,
                    true,
                    [new(CredentialType, request.Password, false)]),
                    cancellationToken);

                return string.IsNullOrWhiteSpace(identityId)
                    ? Result.Failure<string>(Error.NullValue)
                    : Result.Success(identityId);
            }
            catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.Conflict)
            {
                logger.LogError(ex, "User registration failed");
                return Result.Failure<string>(IdentityProviderErrors.EmailIsNotUnique);
            }
        }
    }
}