using Microsoft.Extensions.Options;
using System.Net.Http.Json;
using Users.Application.Abstractions.Identity;

namespace Users.Infrastructure.Identity
{
    internal sealed class KeyCloakClient(HttpClient httpClient, IOptions<KeyCloakOptions> options)
    {
        private readonly KeyCloakOptions _options = options.Value;

        internal async Task<string> RegisterAsync(IdentityProviderUserRequest user, CancellationToken cancellationToken = default)
        {
            var httpResponseMessage = await httpClient.PostAsJsonAsync($"{_options.CurrentRealm}/users", user, cancellationToken).ConfigureAwait(false);

            httpResponseMessage.EnsureSuccessStatusCode();

            return ExtractIdentityIdFromLocationHeader(httpResponseMessage);
        }

        private static string ExtractIdentityIdFromLocationHeader(HttpResponseMessage httpResponseMessage)
        {
            const string UserSegmentName = "users/";

            var locationHeader = httpResponseMessage.Headers.Location?.PathAndQuery;
            if (string.IsNullOrEmpty(locationHeader))
                throw new InvalidOperationException("Location Header is null");

            var userSegmentValueIndex = locationHeader.IndexOf(UserSegmentName, StringComparison.InvariantCultureIgnoreCase);

            return locationHeader[(userSegmentValueIndex + UserSegmentName.Length)..];
        }
    }
}