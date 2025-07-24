namespace Users.Application.Abstractions.Identity
{
    public sealed record UserRequest(
        string Email,
        string FirstName,
        string LastName,
        string Password
        );
}