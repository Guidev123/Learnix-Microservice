using Learning.Domain.Enrollments.Errors;
using Learning.Domain.Enrollments.Interfaces;
using Learnix.Commons.Application.Messaging;
using Learnix.Commons.Domain.Results;

namespace Learning.Application.Enrollments.UseCases.GetCourseContent
{
    internal sealed class GetCourseContentQueryHandler(IEnrollmentRepository enrollmentRepository) : IQueryHandler<GetCourseContentQuery, CourseContentResponse>
    {
        public async Task<Result<CourseContentResponse>> ExecuteAsync(GetCourseContentQuery request, CancellationToken cancellationToken = default)
        {
            var courseContent = await enrollmentRepository.GetCourseByIdAsync(request.CourseId, cancellationToken);
            if (courseContent is null)
            {
                return Result.Failure<CourseContentResponse>(CourseErrors.NotFound(request.CourseId));
            }

            return courseContent.MapFromEntity();
        }
    }
}