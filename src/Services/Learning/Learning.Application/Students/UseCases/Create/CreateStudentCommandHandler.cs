using Learnix.Commons.Application.Messaging;
using Learnix.Commons.Domain.Results;

namespace Learning.Application.Students.UseCases.Create
{
    internal sealed class CreateStudentCommandHandler : ICommandHandler<CreateStudentCommand>
    {
        public Task<Result> ExecuteAsync(CreateStudentCommand request, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}