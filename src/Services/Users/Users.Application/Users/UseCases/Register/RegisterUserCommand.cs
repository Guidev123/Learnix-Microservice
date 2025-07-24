using Learnix.Commons.Application.Abstractions;

namespace Users.Application.Users.UseCases.Register
{
    public sealed record RegisterUserCommand(
        string FullName,
        string Email,
        DateTime BirthDate,
        string Password,
        string ConfirmPassword
        ) : ICommand<RegisterUserResponse>;
}