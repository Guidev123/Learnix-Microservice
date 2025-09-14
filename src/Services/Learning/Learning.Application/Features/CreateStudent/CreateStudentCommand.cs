using Learnix.Commons.Application.Messaging;

namespace Learning.Application.Features.CreateStudent
{
    public sealed record CreateStudentCommand(
        Guid Id,
        string FirstName,
        string LastName,
        string Email
        ) : ICommand;
}