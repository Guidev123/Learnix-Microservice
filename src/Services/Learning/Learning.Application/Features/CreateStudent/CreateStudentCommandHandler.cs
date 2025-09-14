using Learning.Domain.Students.Entities;
using Learning.Domain.Students.Errors;
using Learning.Domain.Students.Interfaces;
using Learnix.Commons.Application.Messaging;
using Learnix.Commons.Domain.Abstractions;
using Learnix.Commons.Domain.Results;

namespace Learning.Application.Features.CreateStudent
{
    internal sealed class CreateStudentCommandHandler(IStudentRepository studentRepository, IUnitOfWork unitOfWork) : ICommandHandler<CreateStudentCommand>
    {
        public async Task<Result> ExecuteAsync(CreateStudentCommand request, CancellationToken cancellationToken = default)
        {
            var alreadyExists = await studentRepository.ExistsAsync(request.Email, cancellationToken);
            if (alreadyExists)
            {
                return Result.Failure(StudentErrors.StudentAlreadyExists(request.Email));
            }

            var student = Student.Create(
                request.Id,
                request.FirstName,
                request.LastName,
                request.Email);

            studentRepository.Insert(student);

            var wasSaved = await unitOfWork.CommitAsync(cancellationToken);
            if (wasSaved is false)
            {
                return Result.Failure(StudentErrors.PersistenceError(request.Id));
            }

            return Result.Success();
        }
    }
}