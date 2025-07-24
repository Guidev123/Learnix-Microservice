namespace Users.Application.Abstractions.Identity
{
    public sealed record IdentityProviderUserRequest(
        string Username,
        string Email,
        string FirstName,
        string LastName,
        bool EmailVerified,
        bool Enabled,
        CredentialRepresentationRequest[] Credentials
        );
}