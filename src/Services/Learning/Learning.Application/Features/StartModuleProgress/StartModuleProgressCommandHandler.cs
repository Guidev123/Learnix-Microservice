using Learning.Domain.Progress.Entities;
using Learning.Domain.Progress.Errors;
using Learning.Domain.Progress.Interfaces;
using Learnix.Commons.Application.Messaging;
using Learnix.Commons.Domain.Abstractions;
using Learnix.Commons.Domain.Results;

namespace Learning.Application.Features.StartModuleProgress
{
    internal sealed class StartModuleProgressCommandHandler(
        ICourseProgressRepository courseProgressRepository,
        IUnitOfWork unitOfWork) : ICommandHandler<StartModuleProgressCommand>
    {
        public async Task<Result> ExecuteAsync(StartModuleProgressCommand request, CancellationToken cancellationToken = default)
        {
            var courseProgress = await courseProgressRepository.GetByStudentAndCourseIdAsync(request.StudentId, request.CourseId, cancellationToken);
            if (courseProgress is null)
            {
                return Result.Failure(CourseProgressErrors.NotFoundForCustomerId(request.StudentId));
            }

            var moduleProgress = ModuleProgress.Create(request.ModuleId, courseProgress.Id);

            courseProgress.UpdateModuleProgress(moduleProgress);

            courseProgressRepository.InsertModuleProgress(moduleProgress);

            var isSuccess = await unitOfWork.CommitAsync(cancellationToken);

            return isSuccess
                ? Result.Success()
                : Result.Failure(CourseProgressErrors.FailToPersistChanges);
        }
    }
}