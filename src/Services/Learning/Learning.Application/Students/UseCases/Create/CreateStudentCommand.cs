using Learnix.Commons.Application.Messaging;

namespace Learning.Application.Students.UseCases.Create
{
    public sealed record CreateStudentCommand(
        Guid Id,
        string FirstName,
        string LastName,
        string Email
        ) : ICommand;
}