using Learning.Domain.Progress.Entities;
using Learning.Domain.Progress.Errors;
using Learning.Domain.Progress.Interfaces;
using Learnix.Commons.Application.Messaging;
using Learnix.Commons.Domain.Abstractions;
using Learnix.Commons.Domain.Results;

namespace Learning.Application.Features.StartLessonProgress
{
    internal sealed class StartLessonProgressCommandHandler(
        ICourseProgressRepository courseProgressRepository,
        IUnitOfWork unitOfWork
        ) : ICommandHandler<StartLessonProgressCommand>
    {
        public async Task<Result> ExecuteAsync(StartLessonProgressCommand request, CancellationToken cancellationToken = default)
        {
            var courseProgress = await courseProgressRepository.GetByStudentAndCourseIdAsync(request.StudentId, request.CourseId, cancellationToken);
            if (courseProgress is null)
            {
                return Result.Failure(CourseProgressErrors.NotFoundForCustomerId(request.StudentId));
            }

            var moduleProgress = courseProgress.GetModuleProgressOrDefault(request.ModuleId);
            if (moduleProgress is null)
            {
                return Result.Failure(ModuleProgressErrors.NotFound(request.ModuleId));
            }

            var lessonProgress = LessonProgress.Create(request.LessonId, request.ModuleId, moduleProgress.Id);
            courseProgress.StartLesson(lessonProgress, request.ModuleId);

            courseProgressRepository.InsertLessonProgress(lessonProgress);

            var isSuccess = await unitOfWork.CommitAsync(cancellationToken);

            return isSuccess ? Result.Success() : Result.Failure(LessonProgressErrors.FailToPersistChanges);
        }
    }
}