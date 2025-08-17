using Courses.Application.CoursesContent.Abstractions;
using Courses.Domain.Courses.Errors;
using Learnix.Commons.Application.Messaging;
using Learnix.Commons.Domain.Results;

namespace Courses.Application.CoursesContent.UseCases.GetContent
{
    internal sealed class GetCourseContentQueryHandler(ICourseContentRepository courseContentRepository) : IQueryHandler<GetCourseContentQuery, CourseContentResponse>
    {
        public async Task<Result<CourseContentResponse>> ExecuteAsync(GetCourseContentQuery request, CancellationToken cancellationToken = default)
        {
            var courseContent = await courseContentRepository.GetByIdAsync(request.CourseId, cancellationToken);
            if (courseContent is null)
            {
                return Result.Failure<CourseContentResponse>(CourseErrors.NotFound(request.CourseId));
            }

            return courseContent;
        }
    }
}