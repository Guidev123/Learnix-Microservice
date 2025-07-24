namespace Users.Application.Abstractions.Identity
{
    public sealed record CredentialRepresentationRequest(
        string Type,
        string Value,
        bool Temporary
        );
}