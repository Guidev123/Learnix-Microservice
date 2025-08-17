using Courses.Application.Courses.Mappers;
using Courses.Domain.Courses.Errors;
using Courses.Domain.Courses.Interfaces;
using Learnix.Commons.Application.Messaging;
using Learnix.Commons.Domain.Results;

namespace Courses.Application.Courses.UseCases.GetContent
{
    internal sealed class GetCourseContentQueryHandler(ICourseRepository courseRepository) : IQueryHandler<GetCourseContentQuery, GetCourseContentResponse>
    {
        public async Task<Result<GetCourseContentResponse>> ExecuteAsync(GetCourseContentQuery request, CancellationToken cancellationToken = default)
        {
            var result = await courseRepository.GetWithModulesAndLessonsAsync(request.CourseId, cancellationToken);
            if (result is null)
            {
                return Result.Failure<GetCourseContentResponse>(CourseErrors.NotFound(request.CourseId));
            }

            return result.MapFromEntity();
        }
    }
}