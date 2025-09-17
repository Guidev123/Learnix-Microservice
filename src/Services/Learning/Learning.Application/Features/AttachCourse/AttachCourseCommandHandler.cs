using Learning.Domain.Enrollments.Interfaces;
using Learnix.Commons.Application.Messaging;
using Learnix.Commons.Domain.Results;

namespace Learning.Application.Features.AttachCourse
{
    internal sealed class AttachCourseCommandHandler(IEnrollmentRepository enrollmentRepository) : ICommandHandler<AttachCourseCommand>
    {
        public async Task<Result> ExecuteAsync(AttachCourseCommand request, CancellationToken cancellationToken = default)
        {
            var course = await enrollmentRepository.GetCourseByIdAsync(request.Id, cancellationToken);

            var courseEntity = request.MapToEntity();

            if (course is null)
            {
                await enrollmentRepository.InsertCourseAsync(courseEntity, cancellationToken);

                return Result.Success();
            }

            await enrollmentRepository.ReplaceCourseAsync(courseEntity, cancellationToken);
            return Result.Success();
        }
    }
}