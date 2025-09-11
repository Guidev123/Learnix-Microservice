using Learning.Domain.Enrollments.Interfaces;
using Learnix.Commons.Application.Messaging;
using Learnix.Commons.Domain.Results;

namespace Learning.Application.Enrollments.UseCases.AttachCourse
{
    internal sealed class AttachCourseCommandHandler(IEnrollmentRepository enrollmentRepository) : ICommandHandler<AttachCourseCommand>
    {
        public async Task<Result> ExecuteAsync(AttachCourseCommand request, CancellationToken cancellationToken = default)
        {
            var course = await enrollmentRepository.GetCourseByIdAsync(request.Id, cancellationToken);
            if (course is null)
            {
                await enrollmentRepository.InsertCourseAsync(request.MapToEntity(), cancellationToken);

                return Result.Success();
            }

            await enrollmentRepository.ReplaceCourseAsync(course, cancellationToken);
            return Result.Success();
        }
    }
}