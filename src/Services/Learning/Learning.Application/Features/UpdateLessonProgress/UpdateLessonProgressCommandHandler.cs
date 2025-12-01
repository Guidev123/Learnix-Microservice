using Learning.Domain.Enrollments.Errors;
using Learning.Domain.Enrollments.Interfaces;
using Learning.Domain.Progress.Errors;
using Learning.Domain.Progress.Interfaces;
using Learnix.Commons.Application.Messaging;
using Learnix.Commons.Domain.Abstractions;
using Learnix.Commons.Domain.Results;

namespace Learning.Application.Features.UpdateLessonProgress
{
    internal sealed class UpdateLessonProgressCommandHandler(
        ICourseProgressRepository courseProgressRepository,
        IEnrollmentRepository enrollmentRepository,
        IUnitOfWork unitOfWork
        ) : ICommandHandler<UpdateLessonProgressCommand>
    {
        public async Task<Result> ExecuteAsync(UpdateLessonProgressCommand request, CancellationToken cancellationToken = default)
        {
            var enrollment = await enrollmentRepository.GetByStudentAndCourseIdAsync(request.StudentId, request.CourseId, cancellationToken);
            if (enrollment is null)
            {
                return Result.Failure(EnrollmentErrors.NotFound(request.StudentId));
            }

            var courseProgress = await courseProgressRepository.GetByStudentAndCourseIdAsync(request.StudentId, request.CourseId, cancellationToken: cancellationToken);
            if (courseProgress is null)
            {
                return Result.Failure(CourseProgressErrors.NotFoundForEnrollmentId(request.StudentId));
            }

            courseProgress.UpdateLessonProgress(
                request.LessonId,
                request.ModuleId,
                request.MinutesWatched,
                request.TotalDuration
            );

            courseProgressRepository.Update(courseProgress);

            var isSuccess = await unitOfWork.CommitAsync(cancellationToken);

            return isSuccess ? Result.Success() : Result.Failure(CourseProgressErrors.FailToPersistChanges);
        }
    }
}