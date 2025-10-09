using Learnix.Commons.Application.Messaging;

namespace Users.Application.Features.Register
{
    public sealed record RegisterUserCommand(
        string FullName,
        string Email,
        DateTime BirthDate,
        string Password,
        string ConfirmPassword
        ) : ICommand<RegisterUserResponse>;
}