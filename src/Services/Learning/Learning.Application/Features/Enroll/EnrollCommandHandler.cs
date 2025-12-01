using Learning.Domain.Enrollments.Entities;
using Learning.Domain.Enrollments.Errors;
using Learning.Domain.Enrollments.Interfaces;
using Learning.Domain.Progress.Entities;
using Learning.Domain.Progress.Errors;
using Learning.Domain.Progress.Interfaces;
using Learning.Domain.Students.Errors;
using Learning.Domain.Students.Interfaces;
using Learnix.Commons.Application.Clock;
using Learnix.Commons.Application.Messaging;
using Learnix.Commons.Domain.Abstractions;
using Learnix.Commons.Domain.Results;

namespace Learning.Application.Features.Enroll
{
    internal sealed class EnrollCommandHandler(
        ICourseProgressRepository courseProgressRepository,
        IEnrollmentRepository enrollmentRepository,
        IStudentRepository studentRepository,
        IUnitOfWork unitOfWork,
        IDateTimeProvider dateTimeProvider) : ICommandHandler<EnrollCommand, EnrollResponse>
    {
        public async Task<Result<EnrollResponse>> ExecuteAsync(EnrollCommand request, CancellationToken cancellationToken = default)
        {
            var student = await studentRepository.GetByIdAsync(request.StudentId, cancellationToken);
            if (student is null)
            {
                return Result.Failure<EnrollResponse>(StudentErrors.NotFound(request.StudentId));
            }

            var enrollment = Enrollment.Create(
                student,
                request.CourseId,
                dateTimeProvider.UtcNow);

            var courseExists = await enrollmentRepository.CourseExistsAsync(request.CourseId, cancellationToken);
            if (courseExists is false)
            {
                return Result.Failure<EnrollResponse>(CourseErrors.NotFound(request.CourseId));
            }

            var courseProgressResult = await InsertCourseProgressAsync(enrollment.StudentId, enrollment.CourseId, cancellationToken);

            if (courseProgressResult.IsFailure)
            {
                return Result.Failure<EnrollResponse>(courseProgressResult.Error!);
            }

            enrollment.SetCourseProgressId(courseProgressResult.Value.Id);

            enrollmentRepository.Insert(enrollment);

            student.Enroll(enrollment, dateTimeProvider.UtcNow);

            var isSuccess = await unitOfWork.CommitAsync(cancellationToken);
            if (isSuccess is false)
            {
                return Result.Failure<EnrollResponse>(EnrollmentErrors.EnrollmentCouldNotBeSaved);
            }

            return new EnrollResponse(enrollment.Id);
        }

        private async Task<Result<CourseProgress>> InsertCourseProgressAsync(
            Guid studentId,
            Guid courseId,
            CancellationToken cancellationToken)
        {
            var alreadyEnrolled = await enrollmentRepository.AlreadyEnrolledAsync(courseId, studentId, cancellationToken);
            if (alreadyEnrolled)
            {
                return Result.Failure<CourseProgress>(EnrollmentErrors.StudentAlreadyEnrolled(studentId, courseId));
            }

            var courseProgress = CourseProgress.Create(studentId, courseId, dateTimeProvider.UtcNow);

            courseProgressRepository.Insert(courseProgress);

            return courseProgress;
        }
    }
}