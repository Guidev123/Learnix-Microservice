namespace Users.Application.Users.UseCases.GetById
{
    public sealed record GetUserByIdResponse(
        Guid Id,
        string FirstName,
        string LastName,
        string Email,
        DateTime BirthDate);
}