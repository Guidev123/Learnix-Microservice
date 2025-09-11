using Learning.Domain.Enrollments.Entities;
using Learning.Domain.Enrollments.Errors;
using Learning.Domain.Enrollments.Interfaces;
using Learning.Domain.Progress.Errors;
using Learning.Domain.Students.Errors;
using Learning.Domain.Students.Interfaces;
using Learnix.Commons.Application.Clock;
using Learnix.Commons.Application.Messaging;
using Learnix.Commons.Domain.Abstractions;
using Learnix.Commons.Domain.Results;

namespace Learning.Application.Enrollments.UseCases.Enroll
{
    internal sealed class EnrollCommandHandler(
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

            var courseExists = await enrollmentRepository.CourseExistsAsync(enrollment.CourseId, cancellationToken);
            if (courseExists is false)
            {
                return Result.Failure<EnrollResponse>(CourseProgressErrors.NotFound(request.CourseId));
            }

            var alreadyEnrolled = await enrollmentRepository.AlreadyEnrolledAsync(enrollment.Id, enrollment.StudentId, cancellationToken);
            if (alreadyEnrolled)
            {
                return Result.Failure<EnrollResponse>(EnrollmentErrors.StudentAlreadyEnrolled(enrollment.StudentId, enrollment.CourseId));
            }

            enrollmentRepository.Insert(enrollment);

            student.Enroll(enrollment, dateTimeProvider.UtcNow);

            var wasSaved = await unitOfWork.CommitAsync(cancellationToken);
            if (wasSaved is false)
            {
                return Result.Failure<EnrollResponse>(EnrollmentErrors.EnrollmentCouldNotBeSaved);
            }

            return new EnrollResponse(enrollment.Id);
        }
    }
}