namespace Users.Application.Features.GetById
{
    public sealed record GetUserByIdResponse(
        Guid Id,
        string FirstName,
        string LastName,
        string Email,
        DateTime BirthDate);
}